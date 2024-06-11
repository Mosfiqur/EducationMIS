import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { UnicefDBSchema } from './UnicefDBSchema';
import { Block } from '../models/idbmodels/indexedDBModels';

@Injectable({
    providedIn: 'root'
})
export class BlockDb {
    constructor(private db: NgxIndexedDBService) {

    }

    getBlocksByCampId(campId:number){
        var keyRange = IDBKeyRange.only(campId);
        return this.db.getAllByIndex(UnicefDBSchema.TableNames.tbl_blocks,
            UnicefDBSchema.ColumnNames.col_camp_id, keyRange);
    }

    getAllBlocks(){
        return this.db.getAll(UnicefDBSchema.TableNames.tbl_blocks);
    }

    getBlockById(id){
        return this.db.getByKey(UnicefDBSchema.TableNames.tbl_blocks, id);
    }

    updateBlock(record:Block){
        return this.db.update(UnicefDBSchema.TableNames.tbl_blocks, record);
    }

    saveBlock(record: Block) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_blocks, record);
    }
}