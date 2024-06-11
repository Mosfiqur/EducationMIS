import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { UnicefDBSchema } from './UnicefDBSchema';
import { ListItem } from '../models/idbmodels/indexedDBModels';

@Injectable({
    providedIn: 'root'
})
export class ListItemDB {
    constructor(private db: NgxIndexedDBService) {

    }

    getListItemById(id){
        return this.db.getByKey(UnicefDBSchema.TableNames.tbl_list_item,id);
    }

    getListItemByListId(listId){
        var keyRange = IDBKeyRange.only(listId);
        return this.db.getAllByIndex(UnicefDBSchema.TableNames.tbl_list_item,
            UnicefDBSchema.ColumnNames.col_list_id, keyRange);
    }

    updateListItem(record:ListItem){
        return this.db.update(UnicefDBSchema.TableNames.tbl_list_item, record);
    }

    saveListItem(record: ListItem) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_list_item, record);
    }
}