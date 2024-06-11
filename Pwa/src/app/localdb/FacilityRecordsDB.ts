import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { UnicefDBSchema } from './UnicefDBSchema';
import { FacilityRecord } from '../models/idbmodels/indexedDBModels';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class FacilityRecordsDB {
    constructor(private db: NgxIndexedDBService) {

    }

    getAllFacilityRecords():Observable<FacilityRecord[]>{
        return this.db.getAll(UnicefDBSchema.TableNames.tbl_facility_records);
    }

    getFacilityRecords(facilityId: number, instanceId: number,columnId: number) {
        let values = [facilityId, instanceId, columnId];
        let keyRange = IDBKeyRange.only(values); 
        return this.db.getAllByIndex(UnicefDBSchema.TableNames.tbl_facility_records,
            UnicefDBSchema.IndexNames.facility_record_index, keyRange);
    }

    updateFacilityRecord(record:FacilityRecord){
        return this.db.update(UnicefDBSchema.TableNames.tbl_facility_records, record);
    }

    saveFacilityRecord(record: FacilityRecord) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_facility_records, record);
    }

    deleteFacilityRecord(id){
        return this.db.delete(UnicefDBSchema.TableNames.tbl_facility_records,id);
    }
}