import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BeneficiaryFilter } from 'src/app/models/beneficiary/beneficiaryFilterModel';
import { FacilityFilter } from 'src/app/models/facility/facilityFilterModel';
import { FacilityStatus } from 'src/app/_enums/facilityStatus';
import { FacilityType } from 'src/app/_enums/facilityType';
import { Gender } from 'src/app/_enums/gender';
import { LevelOfStudy } from 'src/app/_enums/levelOfStudy';
import { TargetPopulation } from 'src/app/_enums/targetedPopulation';

@Component({
    selector: "facility-filter-text",
    templateUrl: "./facilityFilterText.component.html"
})
export class FacilityFilterTextComponent{

    @Input() facilityFilterModel: FacilityFilter;
    @Output() removeFilterClicked = new EventEmitter();

    showCurrentFilter() {
        if (this.facilityFilterModel
            && (
                this.facilityFilterModel.programPartner.length > 0 ||
                this.facilityFilterModel.implementationPartner.length > 0 ||
                this.facilityFilterModel.teachers.length > 0 ||
                this.facilityFilterModel.unionId||
                this.facilityFilterModel.upazilaId ||
                this.facilityFilterModel.targetedPopulation ||
                this.facilityFilterModel.facilityType ||
                this.facilityFilterModel.facilityStatus 
            )) {
            return true;
        }
        else {
            return false;
        }
    }
    showUpazila() {
        if (this.facilityFilterModel.upazilaId)
            return true;
        else
            return false;
    }
    showUnion() {
        if (this.facilityFilterModel.unionId)
            return true;
        else
            return false;
    }
    showTargetedPopulation() {
        if (this.facilityFilterModel.targetedPopulation)
            return true;
        else
            return false;
    }
    showType() {
        if (this.facilityFilterModel.facilityType)
            return true;
        else
            return false;
    }
    showStatus() {
        if (this.facilityFilterModel.facilityStatus)
            return true;
        else
            return false;
    }
    getTargetedPopulation(id) {
        return TargetPopulation[id];
    }
    getType(id) {
        return FacilityType[id];
    }
    getStatus(id) {
        return FacilityStatus[id];
    }
 
    removeFilter(prop){
 
        this.removeFilterClicked.emit(prop);
    }
}