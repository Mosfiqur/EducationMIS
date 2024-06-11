import { MapType } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import {google} from 'google-maps'

import { Globals } from 'src/app/globals';
import { IDropdownSettings } from 'src/app/lib/multi-select';
import { Camp } from 'src/app/models/common/camp.model';
import { IShapeCoordinate } from 'src/app/models/dashboard/gap-map.model';
import { ILcMapQueryModel } from 'src/app/models/dashboard/lc-map-query.model';
import { ILcMapModel, ILearningCenterModel } from 'src/app/models/dashboard/lc-map.model';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { CommonService } from 'src/app/services/common.service';
import { DashboardService } from 'src/app/services/dashboard.service';
import { EducationPartnerService } from 'src/app/services/educationPartner.service';
import { Convert } from 'src/app/utility/Convert';
import { TargetPopulation } from 'src/app/_enums/targetedPopulation';

@Component({
    templateUrl: './lc-map.component.html',
    styleUrls: ['./lc-map.component.scss']
})
export class LcMapComponent implements OnInit{

    private hostFillColor:string = "#7825ad";
    private refugeeFillColor: string = "#f06000";
    
    
    public selectedCamps: Camp[] = [];
    public map: google.maps.Map;
    public bounds = new google.maps.LatLngBounds(); 
    
    public zoomManager = new ZoomLevelManager();

    public lcMaps: ILcMapModel[];

    public dropdownSettings = {};
    public campList: ISelectListItem[] = [];
    public upazilaList: ISelectListItem[] = [];
    public unionList: ISelectListItem[] = [];
    public espList: ISelectListItem[] = [];    
    public tpList: ISelectListItem[] = [];
    private getAllQuery = {
        pageNo: 1, pageSize: this.globals.maxPageSize
    };
    public filters = {
        selectedCamps: [],
        selectedUpazilas: [],
        selectedUnions: [],
        selectedPPs: [],
        selectedIPs: [],
        selectedTPs: [],
        
    }

    constructor(
        private dashboardService: DashboardService,
        private commonService: CommonService,
        private globals: Globals,
        private espService: EducationPartnerService
    ){
        
    }
    ngOnInit(): void {
        this.map = new google.maps.Map(document.getElementById("map") as HTMLElement, {            
            mapTypeId: 'roadmap',            
        }); 
        
        this.dropdownSettings = {
            ...this.globals.multiSelectSettings,
        }as IDropdownSettings;
        
        Promise.all([            
            this.commonService.getAllCamps(),
            this.commonService.getUpazilas(this.getAllQuery),
            this.commonService.getUnions(this.getAllQuery),
            this.espService.getAll()            
        ])
        .then(result => {
            this.campList = result[0].data.map(x => ({id:x.id, text: x.name}));            
            this.upazilaList = result[1].data.map(x => ({id: x.id, text: x.name}));
            this.unionList= result[2].data.map(x => ({id: x.id, text: x.name}));
            this.espList = result[3].map(x => ({id: x.id, text: x.partnerName}));      
            //this.lcMaps = result[4];

            this.tpList = Convert.enumToSelectList(TargetPopulation);
            this.tpList = this.tpList.filter( x=> x.id != TargetPopulation.Both_Communities);            

            this.setAllSelected();
            return this.dashboardService.getLcMap(this.getFilters());            
        })
        .then( response => {
            this.lcMaps = response;
            this.initMap();
        })
        
        ;
    }
    setAllSelected() {
        this.filters.selectedCamps = this.campList.map( x=> x);
        this.filters.selectedIPs = this.espList.map( x=> x);
        this.filters.selectedPPs = this.espList.map( x=> x);
        this.filters.selectedUnions = this.unionList.map( x=> x);
        this.filters.selectedUpazilas = this.upazilaList.map( x=> x);
        this.filters.selectedTPs = this.tpList.map( x=> x);
    }

    initMap(){

        this.lcMaps.forEach((map: ILcMapModel) => {
            this.drawPolygons(map);            
            map.learningCenters.forEach(lc => {                
                this.drawCircle(lc);
                // this.drawMarker(lc); 
            });
        });
        this.fitBounds();
    }
    fitBounds(){
        this.map.fitBounds(this.bounds);          
        this.map.setCenter({lat: 21.1903498643548, lng: 92.15460043860944});
        this.map.setZoom(11);
    }

    drawPolygons = (x: ILcMapModel) => {    
        let paths = x.campCoordinates.map(x=> {
            let latlng = 
            new google.maps.LatLng(x.latitude, x.longitude);
            this.bounds.extend(latlng);
            return latlng;
        });
        let polygon = new google.maps.Polygon({
            paths: paths,
            strokeColor: '#333',
            strokeOpacity: .8,
            strokeWeight: 1,
            fillColor: "#B9DE9D",
            fillOpacity: .5
        });        
        polygon.setMap(this.map);        
    };


    drawCircle(lc: ILearningCenterModel){                   
        let fillColor = lc.targetedPopulation === TargetPopulation.Refugee_Communities ? this.refugeeFillColor : this.hostFillColor;
        
        
        let latlng = new google.maps.LatLng(lc.position.latitude, lc.position.longitude);
        this.bounds.extend(latlng);

        let circle = new google.maps.Circle({
            center:{
                lat: lc.position.latitude,
                lng: lc.position.longitude
            },
            fillColor: fillColor,
            fillOpacity: .8,
            radius: lc.radius * 3,
            strokeColor: '#FFF',
            strokeOpacity: 1,
            strokeWeight: 1,
            zIndex: 100
            
        });
        this.setupInfoWindows(circle, lc);
        //console.log('radius:', circle.getRadius());
        circle.setMap(this.map);
        this.zoomManager.track(circle);        
        google.maps.event.addListener(this.map, 'zoom_changed', () => {            
            var zoomLevel = this.map.getZoom();                   
            this.zoomManager.setCurrentZoomState(circle, zoomLevel);   
        });        
    }

    setupInfoWindows(circle: google.maps.Circle, x: ILearningCenterModel) {


        let infoWindow = new google.maps.InfoWindow({             
           content: `
           <b>Summary</b><br>
           
           Program Partner: ${x.ppName}<br>
           Implementing Partner: ${x.ipName}<br>
           <br>
           Location:<br>
           Upazila: ${x.upazilaName}<br>
           Union: ${x.unionName}<br>
           Camp: ${x.campName}<br>
            
           Facility Code: ${x.facilityCode}<br>
           Number of Beneficiaries: ${x.numberOfBeneficiaries}<br>
           `,
           maxWidth: 600,
           pixelOffset:{width: 0, height: -15, equals: () => true}
       })       
       circle.addListener('mousemove', (event: any) => {            
            infoWindow.close();   
                      
           infoWindow.setPosition(event.latLng);
           infoWindow.open(this.map);
       })           
       circle.addListener('mouseout', (event: any) => {                        
        infoWindow.close();
        })
    };


    drawMarker(lc: ILearningCenterModel){
        let fillColor = lc.targetedPopulation == TargetPopulation.Refugee_Communities ? this.refugeeFillColor : this.hostFillColor;

        let latlng = new google.maps.LatLng(lc.position.latitude, lc.position.longitude);
        this.bounds.extend(latlng);

        let circle = new google.maps.Marker({
            position:{
                lat: lc.position.latitude,
                lng: lc.position.longitude
            },
            icon: {
            path: google.maps.SymbolPath.CIRCLE,    
            scale: 15,
            fillColor: fillColor,
            fillOpacity: .8,            
            strokeColor: '#FFF',
            strokeOpacity: 1,
            strokeWeight: 1            
            }          
        })

        circle.setMap(this.map);
    }

    clear(){
        this.filters.selectedCamps = [];
        this.filters.selectedIPs = [];
        this.filters.selectedPPs = [];
        this.filters.selectedUnions = [];
        this.filters.selectedUpazilas = [];
        this.filters.selectedTPs = [];
    }

    search(){       
        this.dashboardService.getLcMap(this.getFilters())
        .then(x => {
            this.lcMaps = x;
            this.map = new google.maps.Map(document.getElementById("map"), {});
            this.initMap();
            setTimeout(x=> {
                this.fitBounds()
            }, 500);
        });
    }

    getFilters(){
        let filter: ILcMapQueryModel = {
            selectedCamps: this.filters.selectedCamps.map(x => x.id),
            selectedIPs: this.filters.selectedIPs.map( x=> x.id),
            selectedPPs: this.filters.selectedPPs.map(x=> x.id),
            selectedTPs: this.filters.selectedTPs.map(x=> x.id),
            selectedUnions: this.filters.selectedUnions.map(x=> x.id),
            selectedUpazilas: this.filters.selectedUpazilas.map(x=> x.id)
        }
        return filter;
    }
    
}


export class ZoomLevelManager {    
    private threshold : number = 15;
    private factor: number = 2;
    circles: Map<google.maps.Circle, ZoomState> = new Map();
    constructor(){

    }
    track(circle: google.maps.Circle) {
        this.circles.set(circle, new ZoomState(circle.getRadius()));
    }
    setCurrentZoomState(circle: google.maps.Circle, zoomLevel: number) {
        this.circles.get(circle).setCurrentZoomState(zoomLevel);
        let rad = circle.getRadius();           
        if(zoomLevel > this.threshold && this.hasIncreased(circle)){
            circle.setRadius(rad/this.factor);
        }            
        
        if(zoomLevel > this.threshold && this.hasDecreased(circle)){                
            circle.setRadius(rad*this.factor);
        }

        if(zoomLevel <= this.threshold && this.hasDecreased(circle)){
            this.resetRadius(circle);
        }
    }
    hasIncreased(circle: google.maps.Circle):boolean {
        return this.circles.get(circle).hasIncreased();
    }
    hasDecreased(circle: google.maps.Circle):boolean {
        return !this.hasIncreased(circle);
    }    
    resetRadius(circle: google.maps.Circle){
        circle.setRadius(this.circles.get(circle).originalRadius);        
    }

}

export class ZoomState {
    public current: number;
    public previous: number;
        
    constructor(public originalRadius: number){

    }

    hasIncreased():boolean{
        //console.log(`curr: ${this.current}, prev: ${this.previous},  has increased : `, this.current > this.previous);
        return this.current > this.previous;
    }

    setCurrentZoomState(zoomLevel: number){
        this.previous = this.current;
        this.current = zoomLevel;        
        // console.log(`curr: ${this.current}, prev: ${this.previous}`);       
    }
}
