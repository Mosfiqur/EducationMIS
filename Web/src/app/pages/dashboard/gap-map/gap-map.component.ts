import { Component } from '@angular/core';
import {google} from 'google-maps';

import { Globals } from 'src/app/globals';
import { IDropdownSettings } from 'src/app/lib/multi-select';
import { Camp } from 'src/app/models/common/camp.model';
import { IGapMapModel, IShapeCoordinate } from 'src/app/models/dashboard/gap-map.model';
import { IAgeGroup } from 'src/app/models/frameworks/age-group.model';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { CommonService } from 'src/app/services/common.service';
import { DashboardService } from 'src/app/services/dashboard.service';
import { Convert } from 'src/app/utility/Convert';
import { Utility } from 'src/app/utility/Utility';
import { Gender } from 'src/app/_enums/gender';

@Component({
    templateUrl: './gap-map.component.html',
    styleUrls: ['./gap-map.component.scss']
})
export class GapMapComponent{
    public dropdownList = [];
    public selectedItems = [];
    public dropdownSettings = {};
    public ageGroupDropdownSettings = {};
    public campDropdownSettings = {};

    public ageGroupList: IAgeGroup[] = [];
    public genderList: ISelectListItem[] = [];
    public yearList: ISelectListItem[] = [];
    public campList: Camp[];
    

    get showMinGap(): boolean {
        return Math.abs(this.minGap) > 0;
    }
    public map: google.maps.Map;
    public bounds = new google.maps.LatLngBounds();        
    public maxGap: number = 0;
    public minGap: number = 0;
    private gapMaps: IGapMapModel[] = [];
    constructor(
        private dashboardService: DashboardService,
        private globals: Globals,
        private commonService: CommonService
    ){}

    public selectedYears: ISelectListItem[] = [];
    public selectedGenders: ISelectListItem[] = [];
    public selectedAgeGroups: IAgeGroup[] = [];
    public selectedCamps: Camp[] = [];

    get selectedCampIds(): number[]{
        return this.selectedCamps.map(x=> x.id);
    }
    ngOnInit(): void {
        this.map = new google.maps.Map(document.getElementById("map"), {});
        
        this.loadInitialData();
    
        this.setupDrowpdowns();
    }

    loadInitialData(){
      
        this.commonService.getAllCamps()
        .then( x=> {
            this.campList = x.data;
            this.selectedCamps = this.campList.map(x => x);
            return this.commonService.getAllAgeGroups();
        })
        .then( x=> {
            this.ageGroupList = x.data;
            this.selectedAgeGroups = this.ageGroupList.map( x=> x);

            this.genderList = Convert.enumToSelectList(Gender);    
            this.selectedGenders = this.genderList.map(x => x);
            this.yearList = Utility.range(2015, new Date().getFullYear()).sort((a, b) => -1)
                .map(x => ({text: x.toString(), id: x}));
            this.selectedYears = this.yearList.map( x=> x);
            return this.dashboardService.getGapMap(this.getFilters());
        })          
        .then( x=> {
            this.gapMaps = x;                        
            this.initMap();            
        });
        
    }
    setupDrowpdowns(){
        this.dropdownSettings = {
            ...this.globals.multiSelectSettings,
        }as IDropdownSettings;
        this.ageGroupDropdownSettings = {
            ...this.globals.multiSelectSettings,
            idField: 'id',
            textField: 'name',
        }as IDropdownSettings;
        
        this.campDropdownSettings = {
            ...this.globals.multiSelectSettings,
            idField: 'id',
            textField: 'name'
        } as IDropdownSettings;
    }

    initMap() {
        //this.setFakeDataForTesting();
        this.calculateMaxGap();       
        
        this.gapMaps
        .map(this.calculateLatLng)
        .map(this.createPolygons)
        .forEach(this.setupInfoWindows);     

        this.map.fitBounds(this.bounds);                
      }

    calculateMaxGap(){
        this.maxGap = this.gapMaps.map(gapMap => gapMap.gap)
        .reduce((prev, curr, idx) => curr > prev ? curr : prev, 0);     

        this.minGap = Math.min(...this.gapMaps.map(x => x.gap))
    }

    setFakeDataForTesting(){                
        this.gapMaps.forEach( x=> {
            x.outreach = Math.floor(Math.random() * (0 - 8000) ) + 8000
            x.gap = x.target - x.outreach;
        });
    }

    calculateLatLng = (gapMap: IGapMapModel) => {
    let array = gapMap.shapeCoordinates.map( coord => {
        let latlng = new google.maps.LatLng(coord.latitude, coord.longitude);
        this.bounds.extend(latlng);
        return latlng;
    });
    return {
        camp: gapMap,
        latlng: array
    }
    }

    createPolygons = (x) => {              

        let fillColor = x.camp.fillColor;
        let opacity = 1;

        if(this.selectedCamps.findIndex(item=> item.id == x.camp.campId) == -1){
            fillColor = "#B9DE9D"
            opacity = .5;
        }
        let polygon = new google.maps.Polygon({
            paths: x.latlng,
            strokeColor: '#ffddee',
            strokeOpacity: 1,
            strokeWeight: 1,
            fillColor: `${fillColor}`,            
            fillOpacity: opacity
        })
        polygon.setMap(this.map);
        return {
            camp: x.camp,
            polygon: polygon
        };
    };

    setupInfoWindows = x => {
        
        if(this.selectedCampIds.indexOf(x.camp.campId) == -1){
            return;
        }

        let infoWindow = new google.maps.InfoWindow({             
           content: `
           <b>Summary</b><br>
           Camp: ${x.camp.campName}<br>
           PIN: ${x.camp.peopleInNeed}<br>
           Target: ${x.camp.target}<br>
           Outreach: ${x.camp.outreach}<br>
           Gap: ${x.camp.gap}
           `,
           maxWidth: 600,
           pixelOffset:{width: 0, height: -15, equals: () => true}
       })       
       x.polygon.addListener('mousemove', (event: any) => {            
            infoWindow.close();   
                      
           infoWindow.setPosition(event.latLng);
           infoWindow.open(this.map);
       })           
        x.polygon.addListener('mouseout', (event: any) => {                        
        infoWindow.close();
        })
    };

    search(){       
        this.dashboardService.getGapMap(this.getFilters())
        .then(x => {
            this.gapMaps = x;
            this.map = new google.maps.Map(document.getElementById("map"), {});
            this.initMap();
        });
    }

    getFilters(){
        return {
            years: this.selectedYears.map(x => x.id),
            ageGroupIds: this.selectedAgeGroups.map( x=> x.id),
            campIds: this.selectedCamps.map( x=> x.id),
            genders: this.selectedGenders.map( x=> x.id)
        };
    }
    clear(){
        this.selectedAgeGroups = [];
        this.selectedCamps = [];
        this.selectedGenders = [];
        this.selectedYears  =[];
    }
}
