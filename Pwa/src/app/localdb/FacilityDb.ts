import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { UnicefDBSchema } from './UnicefDBSchema';
import { Facility } from '../models/idbmodels/indexedDBModels';

@Injectable({
    providedIn: 'root'
})
export class FacilityDb {
    constructor(private db: NgxIndexedDBService) {

    }

    getAllFacility(){
        return this.db.getAll(UnicefDBSchema.TableNames.tbl_facility);
    }

    getFacilityById(id){
        var keyRange = IDBKeyRange.only(id);
        return this.db.getAllByIndex(UnicefDBSchema.TableNames.tbl_facility,
            UnicefDBSchema.ColumnNames.col_id, keyRange);
    }

    getFacilityByUniqueId(uniqueId){
        return this.db.getByKey(UnicefDBSchema.TableNames.tbl_facility, uniqueId);
    }

    updateFacility(record:Facility){
        return this.db.update(UnicefDBSchema.TableNames.tbl_facility, record);
    }

    saveFacility(record: Facility) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_facility, record);
    }

    deleteFacility(id){
        return this.db.delete(UnicefDBSchema.TableNames.tbl_facility,id);
    }
}