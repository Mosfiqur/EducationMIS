import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { UnicefDBSchema } from './UnicefDBSchema';
import { Beneficiary } from '../models/idbmodels/indexedDBModels';

@Injectable({
    providedIn: 'root'
})
export class BeneficiaryDb {
    constructor(private db: NgxIndexedDBService) {

    }

    getAllBeneficiary() {
        return this.db.getAll(UnicefDBSchema.TableNames.tbl_beneficiary);
    }

    getBeneficiaryByUniqueId(uniqueId) {
        return this.db.getByKey(UnicefDBSchema.TableNames.tbl_beneficiary, uniqueId);
    }

    getBeneficiaryById(id) {
        var keyRange = IDBKeyRange.only(id);
        return this.db.getAllByIndex(UnicefDBSchema.TableNames.tbl_beneficiary,
            UnicefDBSchema.ColumnNames.col_id, keyRange);
    }

    updateBeneficiary(record: Beneficiary) {
        return this.db.update(UnicefDBSchema.TableNames.tbl_beneficiary, record);
    }

    saveBeneficiary(record: Beneficiary) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_beneficiary, record);
    }

    deleteBeneficiary(uniqueId){
        return this.db.delete(UnicefDBSchema.TableNames.tbl_beneficiary,uniqueId);
    }
}