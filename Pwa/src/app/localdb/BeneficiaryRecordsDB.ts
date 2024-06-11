import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { BeneficiaryRecord } from '../models/idbmodels/indexedDBModels';
import { UnicefDBSchema } from './UnicefDBSchema';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
}) 

/**
 * Contains apis for beneficiary records
 */
export class BeneficiaryRecordsDB {
    constructor(private db: NgxIndexedDBService) {

    }

    getAllBeneficiaryRecords():Observable<BeneficiaryRecord[]>{
        return this.db.getAll(UnicefDBSchema.TableNames.tbl_beneficiary_records);
    }

    updateBeneficiaryRecord(record: BeneficiaryRecord){
        return this.db.update(UnicefDBSchema.TableNames.tbl_beneficiary_records,record);
    }

    saveBeneficiaryRecord(record: BeneficiaryRecord) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_beneficiary_records, record);
    }

    getBeneficiaryInstanceRecords(beneficiaryId: number, instanceId: number,columnId: number) {
        let values = [beneficiaryId, instanceId, columnId];
        let keyRange = IDBKeyRange.only(values); 
        return this.db.getAllByIndex(UnicefDBSchema.TableNames.tbl_beneficiary_records,
            UnicefDBSchema.IndexNames.beneficiary_instance, keyRange);
    }

    deleteBeneficiaryRecord(id){
        return this.db.delete(UnicefDBSchema.TableNames.tbl_beneficiary_records,id);
    }
}