import { NgModule } from '@angular/core';
import { DBConfig } from "ngx-indexed-db";
import { UnicefDBSchema } from './UnicefDBSchema';

const dbName = 'unicefedudb';
const dbVersion = 1;

export function migrationFactory() {
    return {
        1: (db, transaction) => {
            const userStore = transaction.objectStore(UnicefDBSchema.TableNames.tbl_user);
            if (!userStore.indexNames.contains(UnicefDBSchema.ColumnNames.col_id)) {
                userStore.createIndex(UnicefDBSchema.ColumnNames.col_id, UnicefDBSchema.ColumnNames.col_id, { unique: false });
            }
            if (!userStore.indexNames.contains(UnicefDBSchema.ColumnNames.col_email)) {
                userStore.createIndex(UnicefDBSchema.ColumnNames.col_email, UnicefDBSchema.ColumnNames.col_email, { unique: false });
            }

            const listItemStore = transaction.objectStore(UnicefDBSchema.TableNames.tbl_list_item);
            if (!listItemStore.indexNames.contains(UnicefDBSchema.ColumnNames.col_list_id)) {
                listItemStore.createIndex(UnicefDBSchema.ColumnNames.col_list_id,
                    UnicefDBSchema.ColumnNames.col_list_id,
                    { unique: false });
            }

            const beneficiaryRecordsStore = transaction.objectStore(UnicefDBSchema.TableNames.tbl_beneficiary_records);
            if (!beneficiaryRecordsStore.indexNames.contains(UnicefDBSchema.IndexNames.beneficiary_instance)) {
                beneficiaryRecordsStore.createIndex(UnicefDBSchema.IndexNames.beneficiary_instance,
                    [UnicefDBSchema.ColumnNames.col_beneficiary_id, UnicefDBSchema.ColumnNames.col_instance_id,
                        UnicefDBSchema.ColumnNames.col_column_id],
                    { unique: true });
            }

            const beneficiaryIndicatorStore = transaction.objectStore(UnicefDBSchema.TableNames.tbl_beneficiary_indicator);
            if (!beneficiaryIndicatorStore.indexNames.contains(UnicefDBSchema.IndexNames.beneficiary_indicator_index)) {
                beneficiaryIndicatorStore.createIndex(UnicefDBSchema.IndexNames.beneficiary_indicator_index,
                    [UnicefDBSchema.ColumnNames.col_entityDynamicColumnId, UnicefDBSchema.ColumnNames.col_instance_id],
                    { unique: true });
            }

            const facilityIndicatorStore = transaction.objectStore(UnicefDBSchema.TableNames.tbl_facility_indicator);
            if (!facilityIndicatorStore.indexNames.contains(UnicefDBSchema.IndexNames.facility_indicator_index)) {
                facilityIndicatorStore.createIndex(UnicefDBSchema.IndexNames.facility_indicator_index,
                    [UnicefDBSchema.ColumnNames.col_entityDynamicColumnId, UnicefDBSchema.ColumnNames.col_instance_id],
                    { unique: true });
            }

            const facilityRecordsStore = transaction.objectStore(UnicefDBSchema.TableNames.tbl_facility_records);
            if (!facilityRecordsStore.indexNames.contains(UnicefDBSchema.IndexNames.facility_record_index)) {
                facilityRecordsStore.createIndex(UnicefDBSchema.IndexNames.facility_record_index,
                    [UnicefDBSchema.ColumnNames.col_facilityId, UnicefDBSchema.ColumnNames.col_instance_id,
                        UnicefDBSchema.ColumnNames.col_column_id],
                    { unique: true });
            }

            const facilityDataCollectionStore = transaction.objectStore(UnicefDBSchema.TableNames.tbl_facility_data_collection_status);
            if (!facilityDataCollectionStore.indexNames.contains(UnicefDBSchema.IndexNames.facility_data_collection_index)) {
                facilityDataCollectionStore.createIndex(UnicefDBSchema.IndexNames.facility_data_collection_index,
                    [UnicefDBSchema.ColumnNames.col_facilityId, UnicefDBSchema.ColumnNames.col_instance_id],
                    { unique: true });
            }

            const beneficiaryDataCollectionStore = transaction.objectStore(UnicefDBSchema.TableNames.tbl_beneficiary_data_collection_status);
            if (!beneficiaryDataCollectionStore.indexNames.contains(UnicefDBSchema.IndexNames.beneficiary_data_collection_index)) {
                beneficiaryDataCollectionStore.createIndex(UnicefDBSchema.IndexNames.beneficiary_data_collection_index,
                    [UnicefDBSchema.ColumnNames.col_beneficiary_id, UnicefDBSchema.ColumnNames.col_instance_id],
                    { unique: true });
            }
        } 
    };
}

export class DBConfiguration {
    static UnicefDbConfig: DBConfig = {
        name: dbName,
        version: dbVersion,
        objectStoresMeta: [UnicefDBSchema.UserTable, UnicefDBSchema.ListTable,
        UnicefDBSchema.ListItemTable, UnicefDBSchema.BeneficiaryRecordsTable,UnicefDBSchema.CampsTable,
        UnicefDBSchema.BlocksTable,UnicefDBSchema.SubBlocksTable,UnicefDBSchema.BeneficiaryScheduleInstanceTable,
        UnicefDBSchema.FacilityScheduleInstanceTable,UnicefDBSchema.FacilityTable,UnicefDBSchema.BeneficiaryTable,
        UnicefDBSchema.FacilityIndicatorTable,UnicefDBSchema.BeneficiaryIndicatorTable,UnicefDBSchema.FacilityRecordsTable,
        UnicefDBSchema.BeneficiaryDataCollectionStatusTable,UnicefDBSchema.FacilityDataCollectionStatusTable
        ],
        migrationFactory
    };
}
