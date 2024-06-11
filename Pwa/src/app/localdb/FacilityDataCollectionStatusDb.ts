import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { UnicefDBSchema } from './UnicefDBSchema';
import { FacilityRecord, FacilityDataCollectionStatus } from '../models/idbmodels/indexedDBModels';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class FacilityDataCollectionStatusDb {
    constructor(private db: NgxIndexedDBService) {

    }

    getAllFacilityDataCollectionStatus():Observable<FacilityDataCollectionStatus[]>{
        return this.db.getAll(UnicefDBSchema.TableNames.tbl_facility_data_collection_status);
    }

    getFacilityDataCollectionStatus(facilityId: number, instanceId: number):Observable<FacilityDataCollectionStatus[]> {
        let values = [facilityId, instanceId];
        let keyRange = IDBKeyRange.only(values); 
        return this.db.getAllByIndex(UnicefDBSchema.TableNames.tbl_facility_data_collection_status,
            UnicefDBSchema.IndexNames.facility_data_collection_index, keyRange);
    }

    updateFacilityDataCollectionStatus(record:FacilityDataCollectionStatus){
        return this.db.update(UnicefDBSchema.TableNames.tbl_facility_data_collection_status, record);
    }

    saveFacilityDataCollectionStatus(record: FacilityDataCollectionStatus) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_facility_data_collection_status, record);
    }

    deleteFacilityDataCollectionStatus(id){
        return this.db.delete(UnicefDBSchema.TableNames.tbl_facility_data_collection_status,id);
    }
}