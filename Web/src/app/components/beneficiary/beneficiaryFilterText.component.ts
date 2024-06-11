import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BeneficiaryFilter } from 'src/app/models/beneficiary/beneficiaryFilterModel';
import { Gender } from 'src/app/_enums/gender';
import { LevelOfStudy } from 'src/app/_enums/levelOfStudy';

@Component({
    selector: "beneficiary-filter-text",
    templateUrl: "./beneficiaryFilterText.component.html"
})
export class BeneficiaryFilterTextComponent{

    @Input() beneficiaryFilterModel: BeneficiaryFilter;
    @Output() removeFilterClicked = new EventEmitter();

    showCurrentFilter() {
        if (this.beneficiaryFilterModel
            && (Object.keys(this.beneficiaryFilterModel.dateOfBirth).length !== 0 ||
                Object.keys(this.beneficiaryFilterModel.enrolmentDate).length !== 0 ||
                this.beneficiaryFilterModel.camps.length > 0 ||
                this.beneficiaryFilterModel.facilities.length > 0 ||
                this.beneficiaryFilterModel.disable ||
                this.beneficiaryFilterModel.levelOfStudy ||
                this.beneficiaryFilterModel.sex
            )) {
            return true;
        }
        else {
            return false;
        }
    }
    showDateOfBirth() {
        if (Object.keys(this.beneficiaryFilterModel.dateOfBirth).length === 0)
            return false;
        else
            return true;
    }
    showEnrolmentDate() {
        if (Object.keys(this.beneficiaryFilterModel.enrolmentDate).length === 0)
            return false;
        else
            return true;
    }
    showSex() {
        if (this.beneficiaryFilterModel.sex)
            return true;
        else
            return false;
    }
    showDisable() {
        
        if (this.beneficiaryFilterModel.disable === undefined)
            return false;
        else
            return true;
    }
    showLevelOfStudy() {
        if (this.beneficiaryFilterModel.levelOfStudy)
            return true;
        else
            return false;
    }
    getGender(id) {
        return Gender[id];
    }
    getLevelOfStudy(id) {
     let levelOfStudy=   [
            { text: "Level 1", id: 1 },
            { text: "Level 2", id: 2 },
            { text: "Level 3", id: 3 },
            { text: "Level 4", id: 4 }
          ]
        return levelOfStudy.filter(a=>a.id==id).map(a=>a.text);
    }

    removeFilter(prop){
        this.removeFilterClicked.emit(prop);
    }
}