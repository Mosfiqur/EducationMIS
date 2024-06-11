import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { UnicefDBSchema } from './UnicefDBSchema';
import { FacilityScheduleInstance } from '../models/idbmodels/indexedDBModels';

@Injectable({
    providedIn: 'root'
})
export class FacilityScheduleInstanceDB {
    constructor(private db: NgxIndexedDBService) {

    }

    getAllFacilityScheduleInstance(){
        return this.db.getAll(UnicefDBSchema.TableNames.tbl_facility_schedule_instance);
    }

    getFacilityScheduleInstanceById(id){
        return this.db.getByKey(UnicefDBSchema.TableNames.tbl_facility_schedule_instance, id);
    }

    updateFacilityScheduleInstance(record:FacilityScheduleInstance){
        return this.db.update(UnicefDBSchema.TableNames.tbl_facility_schedule_instance, record);
    }

    saveFacilityScheduleInstance(record: FacilityScheduleInstance) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_facility_schedule_instance, record);
    }
}