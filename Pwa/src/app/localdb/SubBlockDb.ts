import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { UnicefDBSchema } from './UnicefDBSchema';
import { SubBlock } from '../models/idbmodels/indexedDBModels';

@Injectable({
    providedIn: 'root'
})
export class SubBlockDb {
    constructor(private db: NgxIndexedDBService) {

    }

    getSubBlocksByBlockId(blockId:number){
        var keyRange = IDBKeyRange.only(blockId);
        return this.db.getAllByIndex(UnicefDBSchema.TableNames.tbl_subBlocks,
            UnicefDBSchema.ColumnNames.col_block_id, keyRange);
    }

    getSubBlocks(){
        return this.db.getAll(UnicefDBSchema.TableNames.tbl_subBlocks);
    }

    getSubBlockById(id){
        return this.db.getByKey(UnicefDBSchema.TableNames.tbl_subBlocks, id);
    }

    updateSubBlock(record:SubBlock){
        return this.db.update(UnicefDBSchema.TableNames.tbl_subBlocks, record);
    }

    saveSubBlock(record: SubBlock) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_subBlocks, record);
    }
}