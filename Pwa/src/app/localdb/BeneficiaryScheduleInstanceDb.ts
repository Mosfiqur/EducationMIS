import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { UnicefDBSchema } from './UnicefDBSchema';
import { Block, BeneficiaryScheduleInstance } from '../models/idbmodels/indexedDBModels';

@Injectable({
    providedIn: 'root'
})
export class BeneficiaryScheduleInstanceDb {
    constructor(private db: NgxIndexedDBService) {

    }

    getAllScheduleInstances(){
        return this.db.getAll(UnicefDBSchema.TableNames.tbl_beneficiary_schedule_instance);
    }

    getScheduleInstanceById(id){
        return this.db.getByKey(UnicefDBSchema.TableNames.tbl_beneficiary_schedule_instance, id);
    }

    updateScheduleInstance(record:BeneficiaryScheduleInstance){
        return this.db.update(UnicefDBSchema.TableNames.tbl_beneficiary_schedule_instance, record);
    }

    saveScheduleInstance(record: BeneficiaryScheduleInstance) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_beneficiary_schedule_instance, record);
    }
}