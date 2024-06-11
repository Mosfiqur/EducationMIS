import { Component, ElementRef, EventEmitter, Inject, OnInit, Output, ViewChild } from '@angular/core';
import { BeneficiaryFilter, DateRange } from 'src/app/models/beneficiary/beneficiaryFilterModel';

import * as moment from 'moment';
import { Convert } from 'src/app/utility/Convert';
import { Gender } from 'src/app/_enums/gender';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { LevelOfStudy } from 'src/app/_enums/levelOfStudy';
import { ModalService } from 'src/app/services/modal.service';
import { CampSelectorComponent } from 'src/app/shared/components/camp-selector/camp-selector.component';
import { Camp } from 'src/app/models/common/camp.model';
import { FacilitySelectorComponent } from 'src/app/shared/components/facility-selector/facility-selector.component';
import { FacilityViewModel } from 'src/app/models/facility/facilityViewModel';
import { JQ_TOKEN } from 'src/app/services/jQuery.service';
import { JrpFilter } from 'src/app/models/dashboard/jrp-filter.model';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { InstanceSelectorComponent } from 'src/app/shared/components/instance-selector/instance-selector.component';
import { EducationSectorPartner } from 'src/app/models/educationSectorPartner/educationSectorPartner';
import { PartnerSelectorComponent } from 'src/app/shared/components/partner-selector/partner-selector.component';
import { TargetPopulation } from 'src/app/_enums/targetedPopulation';
import { AgeGroup } from 'src/app/_enums/ageGroup';


@Component({
    selector: "jrp-filter",
    templateUrl: "./jrpFilter.component.html"
})
export class JrpFilterComponent implements OnInit {

    @Output() filterComplete = new EventEmitter();
    @ViewChild('collapseFilter') collapseFilterEl: ElementRef;

    public jrpFilterModel: JrpFilter;

    public genderList: ISelectListItem[] = [];
    public levelOfStudyList: ISelectListItem[] = [];
    public targetPopulationList: ISelectListItem[] = [];
    public ageGroupList: ISelectListItem[] = [];

    public beneficiaryInstances: InstanceViewModel[] = []
    public facilityInstances: InstanceViewModel[] = []
    programPartner: EducationSectorPartner[] = [];
    implementationPartner: EducationSectorPartner[] = [];

    public facilities: FacilityViewModel[] = [];
    public camps: Camp[] = [];

    public disabilityList=[
        {
            title:'Yes',
            value:true
        },
        {
            title:'No',
            value:false
        }
    ]
    constructor(@Inject(JQ_TOKEN) private $: any, private modalService: ModalService) {        
        this.jrpFilterModel = new JrpFilter();
        this.programPartner=[];
        this.implementationPartner=[]
    }
    ngOnInit(): void {
        this.genderList = Convert.enumToSelectList(Gender);
        this.levelOfStudyList = Convert.enumToSelectList(LevelOfStudy);
        this.targetPopulationList=Convert.enumToSelectList(TargetPopulation);
        this.ageGroupList=Convert.enumToSelectList(AgeGroup);
    }

    // instanceModal_clicked() {
    //     var selectedData: InstanceViewModel[] = [];
    //     Object.assign(selectedData, this.instances);

    //     this.modalService
    //         .open<InstanceSelectorComponent, InstanceViewModel[]>(InstanceSelectorComponent, { selectedInstanceIds: selectedData, isMultivalued: "false" })
    //         .then(a => {

    //             this.instances = a;
    //         })
    // }
    beneficiaryInstanceValue_changed(data) {
        this.beneficiaryInstances = data;
        this.jrpFilterModel.beneficiaryInstanceId=data[0].id;
        this.filterComplete.emit(this.jrpFilterModel);
    }
    facilityInstanceValue_changed(data) {
        this.facilityInstances = data;
        this.jrpFilterModel.facilityInstanceId=data[0].id;
        this.filterComplete.emit(this.jrpFilterModel);
    }

    ppModal_clicked() {
        var selectedData: EducationSectorPartner[] = [];

        Object.assign(selectedData, this.programPartner);

        this.modalService
            .open<PartnerSelectorComponent, EducationSectorPartner[]>(PartnerSelectorComponent, { selectedIpIds: selectedData, title: "Program Partner", isMultivalued: "false" })
            .then(a => {

                this.programPartner = a;
                this.jrpFilterModel.programPartnerId=a[0].id.toString();
                this.filterComplete.emit(this.jrpFilterModel);
            })
    }
    removePp(c) {
        var pp = this.programPartner.indexOf(c);
        this.programPartner.splice(pp, 1);
        delete this.jrpFilterModel.programPartnerId
        this.filterComplete.emit(this.jrpFilterModel);
    }

    ipModal_clicked() {
        var selectedData: EducationSectorPartner[] = [];

        Object.assign(selectedData, this.implementationPartner);

        this.modalService
            .open<PartnerSelectorComponent, EducationSectorPartner[]>(PartnerSelectorComponent, { selectedIpIds: selectedData, title: "Implementation Partner", isMultivalued: "false" })
            .then(a => {

                this.implementationPartner = a;
                this.jrpFilterModel.implementationPartnerId=a[0].id.toString();
                this.filterComplete.emit(this.jrpFilterModel);
            })
    }
    removeIp(c) {
        var ip = this.implementationPartner.indexOf(c);
        this.implementationPartner.splice(ip, 1);
        delete this.jrpFilterModel.implementationPartnerId;
        this.filterComplete.emit(this.jrpFilterModel);
    }

    
    campModal_clicked() {
        var selectedData: Camp[] = [];
        Object.assign(selectedData, this.camps);

        this.modalService
            .open<CampSelectorComponent, Camp[]>(CampSelectorComponent, { selectedCampIds: selectedData, isMultivalued: "false" })
            .then(a => {

                this.camps = a;
                this.jrpFilterModel.campId=a[0].id.toString()
                this.filterComplete.emit(this.jrpFilterModel);
            })
    }
    removeBeneficiaryInstance(c) {
        var ins = this.beneficiaryInstances.indexOf(c);
        this.beneficiaryInstances.splice(ins, 1);
        delete this.jrpFilterModel.beneficiaryInstanceId;
        this.filterComplete.emit(this.jrpFilterModel);
    }
    removeFacilityInstance(c) {
        var ins = this.facilityInstances.indexOf(c);
        this.facilityInstances.splice(ins, 1);
        delete this.jrpFilterModel.facilityInstanceId;
        this.filterComplete.emit(this.jrpFilterModel);
    }
    removeCamp(c) {
        var cmp = this.camps.indexOf(c);
        this.camps.splice(cmp, 1);
        delete this.jrpFilterModel.campId;
        this.filterComplete.emit(this.jrpFilterModel);
    }

    applyFilter() {

        this.collapseFilterEl.nativeElement.click();
        // this.beneficiaryFilterModel.camps=[];
        // this.beneficiaryFilterModel.facilities=[];
        // Object.assign(this.beneficiaryFilterModel.camps, this.camps);
        // Object.assign(this.beneficiaryFilterModel.facilities, this.facilities);

        this.filterComplete.emit(this.jrpFilterModel);
    }
    dropDownValueChanged(){        
        this.filterComplete.emit(this.jrpFilterModel);
    }
}