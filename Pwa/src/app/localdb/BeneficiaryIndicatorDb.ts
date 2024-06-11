import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { BeneficiaryIndicator } from '../models/idbmodels/indexedDBModels';
import { UnicefDBSchema } from './UnicefDBSchema';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class BeneficiaryIndicatorDb {
    constructor(private db: NgxIndexedDBService) {

    }
    getAllBeneficiaryIndicators():Observable<BeneficiaryIndicator[]>{
        return this.db.getAll(UnicefDBSchema.TableNames.tbl_beneficiary_indicator);
    }

    getBeneficiaryIndicatorsByInstanceId(instanceId:number) : Observable<BeneficiaryIndicator[]>{
        var keyRange = IDBKeyRange.only(instanceId);
        return this.db.getAllByIndex(UnicefDBSchema.TableNames.tbl_beneficiary_indicator,
            UnicefDBSchema.ColumnNames.col_instance_id, keyRange);
    }

    saveBeneficiaryIndicator(indicator: BeneficiaryIndicator) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_beneficiary_indicator, indicator);
    }

    updateBeneficiaryIndicator(indicator: BeneficiaryIndicator) { 
        return this.db.update(UnicefDBSchema.TableNames.tbl_beneficiary_indicator, indicator);
    }

    getBeneficiaryInstanceIndicators(entityDynamicColumnId: number, instanceId: number) {
        let values = [entityDynamicColumnId, instanceId];
        let keyRange = IDBKeyRange.only(values); 
        return this.db.getAllByIndex(UnicefDBSchema.TableNames.tbl_beneficiary_indicator,
            UnicefDBSchema.IndexNames.beneficiary_indicator_index, keyRange);
    }
}