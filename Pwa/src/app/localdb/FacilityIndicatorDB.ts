import { Injectable } from '@angular/core';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { UnicefDBSchema } from './UnicefDBSchema';
import { FacilityIndicator } from '../models/idbmodels/indexedDBModels';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class FacilityIndicatorDB {
    constructor(private db: NgxIndexedDBService) {

    }

    getFacilityIndicatorsByInstanceId(instanceId:number):Observable<FacilityIndicator[]>{
        var keyRange = IDBKeyRange.only(instanceId);
        return this.db.getAllByIndex(UnicefDBSchema.TableNames.tbl_facility_indicator,
            UnicefDBSchema.ColumnNames.col_instance_id, keyRange);
    }

    getFacilityIndicators(entityDynamicColumnId: number, instanceId: number) {
        let values = [entityDynamicColumnId, instanceId];
        let keyRange = IDBKeyRange.only(values); 
        return this.db.getAllByIndex(UnicefDBSchema.TableNames.tbl_facility_indicator,
            UnicefDBSchema.IndexNames.facility_indicator_index, keyRange);
    }

    updateFacilityIndicator(record:FacilityIndicator){
        return this.db.update(UnicefDBSchema.TableNames.tbl_facility_indicator, record);
    }

    saveFacilityIndicator(record: FacilityIndicator) {
        return this.db.add(UnicefDBSchema.TableNames.tbl_facility_indicator, record);
    }
}