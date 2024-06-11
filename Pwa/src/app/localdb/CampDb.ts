import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { UnicefDBSchema } from './UnicefDBSchema';
import { Camp } from '../models/idbmodels/indexedDBModels';

@Injectable({
    providedIn: 'root'
})
export class CampDb {
    constructor(private db: NgxIndexedDBService) {

    }

    getAllCamps(){
        return this.db.getAll(UnicefDBSchema.TableNames.tbl_camps);
    }

    getCampById(id){
        return this.db.getByKey(UnicefDBSchema.TableNames.tbl_camps, id);
    }

    updateCamp(record:Camp){
        return this.db.update(UnicefDBSchema.TableNames.tbl_camps, record);
    }

    saveCamp(record: Camp) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_camps, record);
    }
}