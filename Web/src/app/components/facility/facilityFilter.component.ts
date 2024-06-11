import { Component, ElementRef, EventEmitter, Inject, OnInit, Output, ViewChild } from '@angular/core';

import { Convert } from 'src/app/utility/Convert';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { ModalService } from 'src/app/services/modal.service';
import { JQ_TOKEN } from 'src/app/services/jQuery.service';
import { CommonService } from 'src/app/services/common.service';
import { FacilityFilter } from 'src/app/models/facility/facilityFilterModel';
import { FacilityType } from 'src/app/_enums/facilityType';
import { FacilityStatus } from 'src/app/_enums/facilityStatus';
import { TargetPopulation } from 'src/app/_enums/targetedPopulation';
import { UnionViewModel } from 'src/app/models/common/unionViewModel';
import { UpazilaViewModel } from 'src/app/models/common/upazilaViewModel';
import { PartnerSelectorComponent } from 'src/app/shared/components/partner-selector/partner-selector.component';
import { EducationSectorPartner } from 'src/app/models/educationSectorPartner/educationSectorPartner';
import { TeacherViewModel } from 'src/app/models/user/teacherViewModel';
import { TeacherSelectorComponent } from 'src/app/shared/components/teacher-selector/teacher-selector.component';


@Component({
    selector: "facility-filter",
    templateUrl: "./facilityFilter.component.html",
    providers: [ModalService]
})
export class FacilityFilterComponent implements OnInit {

    @Output() filterComplete = new EventEmitter();
    @ViewChild('collapseFilter') collapseFilterEl: ElementRef;

    public facilityFilterModel: FacilityFilter;
    public typeList: ISelectListItem[] = [];
    public statusList: ISelectListItem[] = [];
    public tartedPopulationList: ISelectListItem[] = [];
    public allRecords: number = 2147483647;
    unions: UnionViewModel[];
    upazilas: UpazilaViewModel[];

    programPartner: EducationSectorPartner[] = [];
    implementationPartner: EducationSectorPartner[] = [];
    teachers: TeacherViewModel[] = [];

    constructor(private modalService: ModalService
        , private commonService: CommonService) {
        this.facilityFilterModel = new FacilityFilter();
        this.facilityFilterModel.programPartner = [];
        this.facilityFilterModel.implementationPartner = [];
        this.facilityFilterModel.teachers = [];
    }
    ngOnInit(): void {
        this.typeList = Convert.enumToSelectList(FacilityType);
        this.statusList = Convert.enumToSelectList(FacilityStatus);
        this.tartedPopulationList = Convert.enumToSelectList(TargetPopulation);
        this.loadUpazila();
    }
    loadUpazila() {
        this.commonService.getUpazilas({
            pageSize: this.allRecords,
            pageNo: 1
        }).then(data => {
            this.upazilas = data.data;
        })
    }
    selectUpazila(upazilaId) {
        this.upazilas.filter(a => a.id == upazilaId).map(a => this.facilityFilterModel.upazilaName = a.name)
        this.loadUnion(upazilaId);
    }
    selectUnion(unionId) {
        this.unions.filter(a => a.id == unionId).map(a => this.facilityFilterModel.unionName = a.name)
    }
    loadUnion(upazilaId) {
        this.commonService.getUnions({
            pageSize: this.allRecords,
            pageNo: 1,
            upazilaId: upazilaId
        }).then(data => {
            this.unions = data.data;
        })
    }


    teacherModal_clicked() {
        var selectedData: TeacherViewModel[] = [];

        Object.assign(selectedData, this.teachers);

        this.modalService
            .open<TeacherSelectorComponent, TeacherViewModel[]>(TeacherSelectorComponent, { selectedTeacherIds: selectedData, isMultivalued: "true" })
            .then(a => {

                this.teachers = a;
            })
    }
    removeTeacher(c) {
        var t = this.teachers.indexOf(c);
        this.teachers.splice(t, 1);
    }

    ppModal_clicked() {
        var selectedData: EducationSectorPartner[] = [];

        Object.assign(selectedData, this.programPartner);

        this.modalService
            .open<PartnerSelectorComponent, EducationSectorPartner[]>(PartnerSelectorComponent, { selectedIpIds: selectedData, title: "Program Partner", isMultivalued: "true" })
            .then(a => {

                this.programPartner = a;
            })
    }
    removePp(c) {
        var pp = this.programPartner.indexOf(c);
        this.programPartner.splice(pp, 1);
    }

    ipModal_clicked() {
        var selectedData: EducationSectorPartner[] = [];

        Object.assign(selectedData, this.implementationPartner);

        this.modalService
            .open<PartnerSelectorComponent, EducationSectorPartner[]>(PartnerSelectorComponent, { selectedIpIds: selectedData, title: "Implementation Partner", isMultivalued: "true" })
            .then(a => {

                this.implementationPartner = a;
            })
    }
    removeIp(c) {
        var ip = this.implementationPartner.indexOf(c);
        this.implementationPartner.splice(ip, 1);
    }

    loadData(filterModel: FacilityFilter) {
        this.facilityFilterModel.programPartner = [];
        this.facilityFilterModel.implementationPartner = [];
        this.facilityFilterModel.teachers = [];

        Object.assign(this.facilityFilterModel, filterModel);
        this.programPartner = [];
        this.implementationPartner = [];
        this.teachers = []


        Object.assign(this.programPartner, this.facilityFilterModel.programPartner);
        Object.assign(this.implementationPartner, this.facilityFilterModel.implementationPartner);
        Object.assign(this.teachers, this.facilityFilterModel.teachers);
        if(filterModel.unionId!=null){
            this.loadUnion(filterModel.upazilaId);
        }
    }

    removeFilter(prop) {
        if (prop === "programPartner" || prop === "implementationPartner" || prop === "teachers") {
            this.removeList(prop);
        }
        else if (prop === "upazila" || prop === "union") {
            this.removeProperties(prop + "Id");
            this.removeProperties(prop + "Name");
        }
        else if (prop === "targetedPopulation" || prop === "facilityType" || prop === "facilityStatus") {
            this.removeProperties(prop);
        }

    }

    removeProperties(prop: string) {
        delete this.facilityFilterModel[prop];
    }

    removeList(prop: string) {

        if (prop === "programPartner") {
            this.programPartner = [];
        }
        if (prop === "implementationPartner") {
            this.implementationPartner = [];
        }
        if (prop === "teachers") {
            this.teachers = [];
        }
        this.facilityFilterModel[prop] = [];
    }
    applyFilter() {

        this.collapseFilterEl.nativeElement.click();
        this.facilityFilterModel.programPartner = [];
        this.facilityFilterModel.implementationPartner = [];
        this.facilityFilterModel.teachers = [];

        Object.assign(this.facilityFilterModel.programPartner, this.programPartner);
        Object.assign(this.facilityFilterModel.implementationPartner, this.implementationPartner);
        Object.assign(this.facilityFilterModel.teachers, this.teachers);
        this.filterComplete.emit(this.facilityFilterModel);
    }
}