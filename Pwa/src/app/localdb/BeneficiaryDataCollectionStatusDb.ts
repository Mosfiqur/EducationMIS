import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { UnicefDBSchema } from './UnicefDBSchema';
import { FacilityRecord, BeneficiaryDataCollectionStatus } from '../models/idbmodels/indexedDBModels';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class BeneficiaryDataCollectionStatusDb {
    constructor(private db: NgxIndexedDBService) {

    }

    getAllBeneficiaryDataCollectionStatus():Observable<BeneficiaryDataCollectionStatus[]>{
        return this.db.getAll(UnicefDBSchema.TableNames.tbl_beneficiary_data_collection_status);
    }

    getBeneficiaryDataCollectionStatus(beneficiaryId: number, instanceId: number):Observable<BeneficiaryDataCollectionStatus[]> {
        let values = [beneficiaryId, instanceId];
        let keyRange = IDBKeyRange.only(values); 
        return this.db.getAllByIndex(UnicefDBSchema.TableNames.tbl_beneficiary_data_collection_status,
            UnicefDBSchema.IndexNames.beneficiary_data_collection_index, keyRange);
    }

    updateBeneficiaryDataCollectionStatus(record:BeneficiaryDataCollectionStatus){
        return this.db.update(UnicefDBSchema.TableNames.tbl_beneficiary_data_collection_status, record);
    }

    saveBeneficiaryDataCollectionStatus(record: BeneficiaryDataCollectionStatus) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_beneficiary_data_collection_status, record);
    }

    deleteBeneficiaryDataCollectionStatus(id){
        return this.db.delete(UnicefDBSchema.TableNames.tbl_beneficiary_data_collection_status,id);
    }
}