import { Component, ElementRef, EventEmitter, Inject, Input, OnInit, Output, ViewChild } from '@angular/core';
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


@Component({
    selector: "beneficiary-filter",
    templateUrl: "./beneficiaryFilter.component.html"
})
export class BeneficiaryFilterComponent implements OnInit {

    @Output() filterComplete = new EventEmitter();
    @ViewChild('collapseFilter') collapseFilterEl: ElementRef;

    public beneficiaryFilterModel: BeneficiaryFilter;
    public genderList: ISelectListItem[] = [];
    public levelOfStudyList: ISelectListItem[] = [];

    public facilities: FacilityViewModel[] = [];
    public camps: Camp[] = [];
    listOfDisability = [
        { name: "Yes", value: true },
        { name: "No", value: false }
    ]

    constructor(@Inject(JQ_TOKEN) private $: any, private modalService: ModalService) {
        this.beneficiaryFilterModel = new BeneficiaryFilter();
        this.beneficiaryFilterModel.camps = [];
        this.beneficiaryFilterModel.facilities = [];
        this.beneficiaryFilterModel.dateOfBirth = new DateRange();
        this.beneficiaryFilterModel.enrolmentDate = new DateRange();

    }
    ngOnInit(): void {
        this.genderList = Convert.enumToSelectList(Gender);
        this.levelOfStudyList = [
            { text: "Level 1", id: 1 },
            { text: "Level 2", id: 2 },
            { text: "Level 3", id: 3 },
            { text: "Level 4", id: 4 }
          ]
    }
    dateOfBirthUpdated(event) {
        this.beneficiaryFilterModel.dateOfBirth.startDate = moment(event.startDate).format('DD-MM-YYYY');
        this.beneficiaryFilterModel.dateOfBirth.endDate = moment(event.endDate).format('DD-MM-YYYY');
    }
    dobUpdated(event) {

        this.beneficiaryFilterModel.dateOfBirth.startDate = moment(event.startDate, 'DD-MM-YYYY').format('DD-MM-YYYY');
        this.beneficiaryFilterModel.dateOfBirth.endDate = moment(event.endDate, 'DD-MM-YYYY').format('DD-MM-YYYY');
    }
    enrollmentDatehUpdated(event) {
        this.beneficiaryFilterModel.enrolmentDate.startDate = moment(event.startDate).format('DD-MM-YYYY');
        this.beneficiaryFilterModel.enrolmentDate.endDate = moment(event.endDate).format('DD-MM-YYYY');
    }
    eDatehUpdated(event) {
        this.beneficiaryFilterModel.enrolmentDate.startDate = moment(event.startDate, 'DD-MM-YYYY').format('DD-MM-YYYY');
        this.beneficiaryFilterModel.enrolmentDate.endDate = moment(event.endDate, 'DD-MM-YYYY').format('DD-MM-YYYY');
    }
    campModal_clicked() {
        var selectedData: Camp[] = [];
        Object.assign(selectedData, this.camps);

        this.modalService
            .open<CampSelectorComponent, Camp[]>(CampSelectorComponent, { selectedCampIds: selectedData, isMultivalued: "true" })
            .then(a => {

                this.camps = a;
            })
    }
    removeCamp(c) {
        var cmp = this.camps.indexOf(c);
        this.camps.splice(cmp, 1);
    }

    facilityModal_clicked() {
        var selectedData: FacilityViewModel[] = [];
        Object.assign(selectedData, this.facilities);

        this.modalService
            .open<FacilitySelectorComponent, FacilityViewModel[]>(FacilitySelectorComponent, { selectedFacilityIds: selectedData, isMultivalued: "true" })
            .then(a => {

                this.facilities = a;
            })
    }

    loadData(filterModel: BeneficiaryFilter) {
        this.beneficiaryFilterModel = new BeneficiaryFilter();
        this.beneficiaryFilterModel.camps = [];
        this.beneficiaryFilterModel.facilities = [];
        this.beneficiaryFilterModel.dateOfBirth = new DateRange();
        this.beneficiaryFilterModel.enrolmentDate = new DateRange();

        Object.assign(this.beneficiaryFilterModel, filterModel);

        this.camps = [];
        this.facilities = [];
        Object.assign(this.camps, this.beneficiaryFilterModel.camps);
        Object.assign(this.facilities, this.beneficiaryFilterModel.facilities);
        if (this.beneficiaryFilterModel.dateOfBirth.startDate != null) {
            this.dobUpdated(this.beneficiaryFilterModel.dateOfBirth);
            this.$('#dateOfBirth').val(this.beneficiaryFilterModel.dateOfBirth.startDate + " - " + this.beneficiaryFilterModel.dateOfBirth.endDate);
        }
        if (this.beneficiaryFilterModel.enrolmentDate.startDate != null) {
            this.eDatehUpdated(this.beneficiaryFilterModel.enrolmentDate);
            this.$('#enrolmentDate').val(this.beneficiaryFilterModel.enrolmentDate.startDate + " - " + this.beneficiaryFilterModel.enrolmentDate.endDate);
        }
    }

    removeFacility(c) {
        var fac = this.facilities.indexOf(c);
        this.facilities.splice(fac, 1);
    }

    removeFilter(prop) {
        if (prop === "camps" || prop === "facilities") {
            this.removeList(prop);
        }
        else if (prop === "sex" || prop === "disable" || prop === "levelOfStudy") {
            this.removeProperties(prop);
        }
        else if (prop === "dateOfBirth" || prop === "enrolmentDate") {
            this.removeDate(prop);
        }
    }

    removeProperties(prop: string) {
        delete this.beneficiaryFilterModel[prop];
    }
    removeDate(prop: string) {
        this.beneficiaryFilterModel[prop] = new DateRange();
        var a = this.$('#' + prop).val();
        this.$('#' + prop).val("");
    }
    removeList(prop: string) {
        if (prop === "camps") {
            this.camps = [];
        }
        if (prop === "facilities") {
            this.facilities = [];
        }

        this.beneficiaryFilterModel[prop] = [];
    }
    applyFilter() {

        this.collapseFilterEl.nativeElement.click();
        this.beneficiaryFilterModel.camps = [];
        this.beneficiaryFilterModel.facilities = [];
        Object.assign(this.beneficiaryFilterModel.camps, this.camps);
        Object.assign(this.beneficiaryFilterModel.facilities, this.facilities);

        this.filterComplete.emit(this.beneficiaryFilterModel);
    }
}