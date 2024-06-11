import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { UnicefDBSchema } from './UnicefDBSchema';
import { List } from '../models/idbmodels/indexedDBModels';

@Injectable({
    providedIn: 'root'
})
export class ListDb {
    constructor(private db: NgxIndexedDBService) {

    }

    getListById(id){
        var keyRange = IDBKeyRange.only(id);
        return this.db.getByID(UnicefDBSchema.TableNames.tbl_list, id);
    }

    updateList(record:List){
        return this.db.update(UnicefDBSchema.TableNames.tbl_list, record);
    }

    saveList(record: List) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_list, record);
    }
}