import { Injectable } from '@angular/core';
import { ListItem, List, FacilityIndicator, FacilityRecord, FacilityScheduleInstance, BeneficiaryRecord, BeneficiaryIndicator, Beneficiary, Facility, BeneficiaryScheduleInstance, Camp, Block, SubBlock, FacilityDataCollectionStatus, BeneficiaryDataCollectionStatus } from '../models/idbmodels/indexedDBModels';
import { CampDb } from './CampDb';
import { BlockDb } from './BlockDb';
import { SubBlockDb } from './SubBlockDb';
import { BeneficiaryScheduleInstanceDb } from './BeneficiaryScheduleInstanceDb';
import { FacilityDb } from './FacilityDb';
import { BeneficiaryDb } from './BeneficiaryDb';
import { BeneficiaryIndicatorDb } from './BeneficiaryIndicatorDb';
import { BeneficiaryRecordsDB } from './BeneficiaryRecordsDB';
import { FacilityScheduleInstanceDB } from './FacilityScheduleInstanceDB';
import { FacilityIndicatorDB } from './FacilityIndicatorDB';
import { FacilityRecordsDB } from './FacilityRecordsDB';
import { ListDb } from './ListDb';
import { ListItemDB } from './ListItemDB';
import { CollectionStatus } from '../_enums/collectionStatus';
import { BeneficiaryDataCollectionStatusDb } from './BeneficiaryDataCollectionStatusDb';
import { FacilityDataCollectionStatusDb } from './FacilityDataCollectionStatusDb';

@Injectable({
    providedIn: 'root'
})
export class IndexedDbService {
    constructor(private campDbService: CampDb, private blockDbService: BlockDb, private subBlockDbService: SubBlockDb,
        private beneficiaryInstanceService: BeneficiaryScheduleInstanceDb, private facilityService: FacilityDb,
        private beneficiaryDbService: BeneficiaryDb, private beneficiaryIndicatorDbService: BeneficiaryIndicatorDb,
        private beneficiaryRecordsDbService: BeneficiaryRecordsDB, private facilityScheduleInstanceDbService: FacilityScheduleInstanceDB,
        private facilityIndicatorDbService: FacilityIndicatorDB, private facilityRecordsDbService: FacilityRecordsDB,
        private listDbService: ListDb, private listItemDbService: ListItemDB,
        private facilityDataCollectionStatusDbService:FacilityDataCollectionStatusDb,private beneficiaryDataCollectionDbService:BeneficiaryDataCollectionStatusDb) {

    }

    SaveFacilityDataCollectionStatus(record: FacilityDataCollectionStatus,isDownloading:Boolean) {
        return new Promise<number>(async (resolve, reject) => {
            await this.facilityDataCollectionStatusDbService.getFacilityDataCollectionStatus(record.facilityId,
                record.instanceId).subscribe((records) => {

                    if (!(records && records.length > 0)) {
                        this.facilityDataCollectionStatusDbService.saveFacilityDataCollectionStatus(record)
                            .subscribe((key) => {
                                resolve(key);
                            });
                    }
                    else {
                        if (!isDownloading) {
                            let dbRecord = records[0];
                            dbRecord.status = record.status;
                            this.facilityDataCollectionStatusDbService.updateFacilityDataCollectionStatus(dbRecord)
                                .subscribe((facilityRecords) => {
                                    resolve(dbRecord.id);
                                });
                        }
                        else if (isDownloading) {
                            resolve(records[0].id);
                        }
                    }
                });
        })
    }

    SaveBeneficiaryDataCollectionStatus(record: BeneficiaryDataCollectionStatus,isDownloading:Boolean) {
        return new Promise<number>(async (resolve, reject) => {
            await this.beneficiaryDataCollectionDbService.getBeneficiaryDataCollectionStatus(record.beneficiaryId,
                record.instanceId).subscribe((records) => {

                    if (!(records && records.length > 0)) {
                        this.beneficiaryDataCollectionDbService.saveBeneficiaryDataCollectionStatus(record)
                            .subscribe((key) => {
                                resolve(key);
                            });
                    }
                    else {
                        if (!isDownloading) {
                            let dbRecord = records[0];
                            dbRecord.status = record.status;
                            this.beneficiaryDataCollectionDbService.updateBeneficiaryDataCollectionStatus(dbRecord)
                                .subscribe((beneficiaryRecords) => {
                                    resolve(dbRecord.id);
                                });
                        }
                        else if (isDownloading) {
                            resolve(records[0].id);
                        }
                    }
                });
        })
    }

    SaveListItem(listItem: ListItem) {
        return new Promise<number>((resolve, reject) => {
            this.listItemDbService.getListItemById(listItem.id).subscribe((dbResult) => {

                if (dbResult) {

                    dbResult.title = listItem.title;
                    dbResult.value = listItem.value;

                    this.listItemDbService.updateListItem(dbResult)
                        .subscribe((fullListResult) => {
                            resolve(dbResult.id);
                        })
                }
                else {

                    this.listItemDbService.saveListItem(listItem)
                        .subscribe((key) => {
                            resolve(key);
                        })
                }
            });
        })
    }

    SaveList(list: List) {
        return new Promise<any>((resolve, reject) => {
            this.listDbService.getListById(list.id).subscribe(async (dbResult) => {

                if (dbResult) {
                    dbResult.name = list.name;

                    await this.listDbService.updateList(dbResult)
                        .subscribe((fullListResult) => {
                            resolve(dbResult.id);
                        })
                }
                else {
                    await this.listDbService.saveList(list)
                        .subscribe((key) => {
                            resolve(key);
                        })
                }
            });
        })
    }

    SaveFacilityIndicator(facilityIndicator: FacilityIndicator) {

        return new Promise<FacilityIndicator>(async (resolve, reject) => {
            await this.facilityIndicatorDbService.getFacilityIndicators(facilityIndicator.entityDynamicColumnId,
                facilityIndicator.instanceId).subscribe(async (indicators) => {

                    if (indicators && indicators.length > 0) {
                        let dbIndicator = indicators[0];
                        dbIndicator.columnName = facilityIndicator.columnName;
                        dbIndicator.columnNameInBangla = facilityIndicator.columnNameInBangla;
                        dbIndicator.columnDataType = facilityIndicator.columnDataType;
                        dbIndicator.isMultiValued = facilityIndicator.isMultiValued;
                        dbIndicator.listId = facilityIndicator.listId;

                        await this.facilityIndicatorDbService.updateFacilityIndicator(dbIndicator)// when updating returns all table Record
                            .subscribe((facilityIndicatorResult) => {
                                //console.log("Updated Facility Indicator Record Id :", facilityIndicatorResult);
                                resolve(indicators[0]);
                            });
                    }
                    else {
                        await this.facilityIndicatorDbService.saveFacilityIndicator(facilityIndicator)
                            .subscribe((key) => {
                                //console.log("Saved Facility Indicator Record Id :", key);
                                this.facilityIndicatorDbService.getFacilityIndicators(facilityIndicator.entityDynamicColumnId,
                                    facilityIndicator.instanceId).subscribe((resultIndicator) => {
                                        resolve(resultIndicator[0]);
                                    })
                            })
                    }
                })
        })
    }

    SaveFacilityRecord(facilityRecord: FacilityRecord, isDownloading: boolean) {
        return new Promise<number>(async (resolve, reject) => {
            await this.facilityRecordsDbService.getFacilityRecords(facilityRecord.facilityId,
                facilityRecord.instanceId, facilityRecord.columnId).subscribe((records) => {

                    if (!(records && records.length > 0)) {
                        this.facilityRecordsDbService.saveFacilityRecord(facilityRecord)
                            .subscribe((key) => {
                                resolve(key);
                            });
                    }
                    else {
                        if (!isDownloading) {
                            let dbRecord = records[0];
                            dbRecord.status = facilityRecord.status;
                            dbRecord.value = facilityRecord.value;
                            this.facilityRecordsDbService.updateFacilityRecord(dbRecord)
                                .subscribe((facilityRecords) => {
                                    resolve(dbRecord.id);
                                });
                        }
                        else if (isDownloading) {
                            resolve(records[0].id);
                        }
                    }
                });
        })
    }


    SaveFacilityScheduleInstance(instance: FacilityScheduleInstance) {
        this.facilityScheduleInstanceDbService.getFacilityScheduleInstanceById(instance.id).subscribe((result) => {
            if (result) {
                this.facilityScheduleInstanceDbService.updateFacilityScheduleInstance(instance)
                    .subscribe((results) => {
                        console.log('Updated Schedule : ', results);
                    })
            }
            else {
                this.facilityScheduleInstanceDbService.saveFacilityScheduleInstance(instance)
                    .subscribe((key) => {
                        console.log('Created Facility Schedule id: ', key);
                    })
            }
        });
    }

    SaveBeneficiaryRecord(beneficiaryRecord: BeneficiaryRecord, isDownloading: Boolean) {
        return new Promise<any>((resolve, reject) => {

            this.beneficiaryRecordsDbService.getBeneficiaryInstanceRecords(beneficiaryRecord.beneficiaryId,
                beneficiaryRecord.instanceId, beneficiaryRecord.columnId).subscribe((records) => {

                    if (!(records && records.length > 0)) {
                        this.beneficiaryRecordsDbService.saveBeneficiaryRecord(beneficiaryRecord)
                            .subscribe((key) => {
                                //console.log("Saved Beneficiary Record : ", key);
                                resolve(key);
                            });
                    }
                    else {
                        if (!isDownloading) {
                            let dbRecord = records[0];
                            dbRecord.status = beneficiaryRecord.status;
                            dbRecord.value = beneficiaryRecord.value;
                            this.beneficiaryRecordsDbService.updateBeneficiaryRecord(dbRecord)
                                .subscribe((facilityRecords) => {
                                    resolve(dbRecord.id);
                                });
                        }
                        else if (isDownloading) {
                            resolve(records[0].id);
                        }
                    }
                })
        })
    }

    async SaveBenefeciaryIndicator(indicator: BeneficiaryIndicator) {
        return new Promise<BeneficiaryIndicator>(async (resolve, reject) => {
            await this.beneficiaryIndicatorDbService.getBeneficiaryInstanceIndicators(indicator.entityDynamicColumnId,
                indicator.instanceId).subscribe(async (indicators) => {

                    if (indicators && indicators.length > 0) {
                        let dbIndicator = indicators[0];
                        dbIndicator.columnName = indicator.columnName;
                        dbIndicator.columnNameInBangla = indicator.columnNameInBangla;
                        dbIndicator.columnDataType = indicator.columnDataType;
                        dbIndicator.isMultiValued = indicator.isMultiValued;
                        dbIndicator.listId = indicator.listId;

                        await this.beneficiaryIndicatorDbService.updateBeneficiaryIndicator(dbIndicator)// when updating returns all table Record
                            .subscribe((beneficiaryIndicatorResult) => {
                                //console.log("Updated Beneficiary Indicator Record Id :", beneficiaryIndicatorResult);
                                resolve(indicators[0]);
                            });
                    }
                    else {
                        await this.beneficiaryIndicatorDbService.saveBeneficiaryIndicator(indicator)
                            .subscribe((key) => {
                                //console.log("Saved Beneficiary Indicator Record Id :", key);

                                this.beneficiaryIndicatorDbService.getBeneficiaryInstanceIndicators(indicator.entityDynamicColumnId,
                                    indicator.instanceId).subscribe(async (indicators) => {
                                        resolve(indicators[0]);
                                    })
                            })
                    }
                })
        })
    }

    SaveBeneficiary(beneficiary: Beneficiary) {
        return new Promise<Beneficiary>(async (resolve, reject) => {
            this.beneficiaryDbService.getBeneficiaryById(beneficiary.id).subscribe(async (result) => {

                if (result && result.length > 0) {
                    let dbResult = result[0];
                    dbResult.UnhcrId = beneficiary.UnhcrId;
                    dbResult.beneficiaryName = beneficiary.beneficiaryName;
                    dbResult.fatherName = beneficiary.fatherName;
                    dbResult.motherName = beneficiary.motherName;
                    dbResult.FCNId = beneficiary.FCNId;
                    dbResult.dateOfBirth = beneficiary.dateOfBirth;
                    dbResult.sex = beneficiary.sex;
                    dbResult.disabled = beneficiary.disabled;
                    dbResult.disengaged = beneficiary.disengaged;
                    dbResult.levelOfStudy = beneficiary.levelOfStudy;
                    dbResult.enrollmentDate = beneficiary.enrollmentDate;
                    dbResult.facilityId = beneficiary.facilityId;
                    dbResult.beneficiaryCampId = beneficiary.beneficiaryCampId;
                    dbResult.blockId = beneficiary.blockId;
                    dbResult.subBlockId = beneficiary.subBlockId;
                    dbResult.collectionStatus = beneficiary.collectionStatus;
                    await this.beneficiaryDbService.updateBeneficiary(dbResult)
                        .subscribe((beneficiaryResult) => {
                            //console.log('Updated Beneficiary id: ', dbResult.id);

                            var currentBeneficiary = beneficiaryResult.filter(x => x.id == beneficiary.id)[0];
                            resolve(currentBeneficiary);
                        })
                }
                else {
                    await this.beneficiaryDbService.saveBeneficiary(beneficiary)
                        .subscribe((key) => {

                            //console.log('Saved Beneficiary id: ', key);
                            this.beneficiaryDbService.getBeneficiaryByUniqueId(key).subscribe((beneficaryResult) => {
                                resolve(beneficaryResult);
                            })
                        })
                }
            });
        })
    }

    SaveFacility(facility: Facility) {
        return new Promise<Facility>((resolve, reject) => {
            this.facilityService.getFacilityById(facility.id).subscribe(async (dbResult) => {
                if (dbResult && dbResult.length > 0) {
                    let facilityDbResult = dbResult[0];
                    facilityDbResult.campName = facility.campName;
                    facilityDbResult.facilityCode = facility.facilityCode;
                    facilityDbResult.facilityName = facility.facilityName;
                    facilityDbResult.implementationPartnerName = facility.implementationPartnerName;
                    facilityDbResult.programmingPartnerName = facility.programmingPartnerName;

                    this.facilityService.updateFacility(facilityDbResult)
                        .subscribe((key) => {
                            resolve(facilityDbResult);
                            //console.log('Updated Facility Record id: ', key);
                        })
                }
                else {
                    this.facilityService.saveFacility(facility)
                        .subscribe((key) => {

                            this.facilityService.getFacilityByUniqueId(key).subscribe((result) => {
                                resolve(result);
                            })
                            //console.log('Saved Facility Record id: ', key);
                        })
                }
            });
        })
    }

    SaveScheduleInstance(instance: BeneficiaryScheduleInstance) {
        return new Promise<any>((resolve, reject) => {

            this.beneficiaryInstanceService.getScheduleInstanceById(instance.id).subscribe((result) => {
                if (result) {
                    this.beneficiaryInstanceService.updateScheduleInstance(instance)
                        .subscribe((key) => {
                            resolve(instance.id);
                            // console.log('Updated Schedule id: ', key);
                        })
                }
                else {
                    this.beneficiaryInstanceService.saveScheduleInstance(instance)
                        .subscribe((key) => {
                            resolve(instance.id);
                            // console.log('Created Schedule id: ', key);
                        })
                }
            });
        })
    }

    async SaveCamp(camp: Camp) {
        return new Promise<number>(async (resolve, reject) => {
            this.campDbService.getCampById(camp.id).subscribe((result) => {
                if (result) {
                    this.campDbService.updateCamp(camp)
                        .subscribe((camps) => {
                            resolve(camp.id);
                        })
                }
                else {
                    this.campDbService.saveCamp(camp)
                        .subscribe((key) => {
                            resolve(camp.id);
                        })
                }
            });
        });
    }

    async SaveBlock(block: Block) {
        return new Promise<number>(async (resolve, reject) => {
            this.blockDbService.getBlockById(block.id).subscribe((result) => {
                if (result) {
                    this.blockDbService.updateBlock(block)
                        .subscribe((key) => {
                            resolve(block.id);
                            //console.log('Updated Block Record id: ', key);
                        })
                }
                else {
                    this.blockDbService.saveBlock(block)
                        .subscribe((key) => {
                            resolve(block.id);
                            //console.log('Saved Block Record id: ', key);
                        })
                }
            });
        });
    }

    async SaveSubBlock(subBlock: SubBlock) {
        return new Promise<number>(async (resolve, reject) => {
            await this.subBlockDbService.getSubBlockById(subBlock.id).subscribe((result) => {
                if (result) {
                    this.subBlockDbService.updateSubBlock(subBlock)
                        .subscribe((key) => {
                            resolve(subBlock.id);
                            //console.log('Updated SubBlock Record id: ', key);
                        })
                }
                else {
                    this.subBlockDbService.saveSubBlock(subBlock)
                        .subscribe((key) => {
                            resolve(subBlock.id);
                            //console.log('Saved SubBlock Record id: ', key);
                        })
                }
            });
        });
    }

    // FacilityIndicator - (id , entityDynamicColumnId) , FacilityRecord - (columnId)
    // dataResult : FacilityRecord[] 
    // entityDynamicCOlumnId >= 93 && <= 102
    // dataResult theke oi id gula falaye dite hobe match korar por 


    private checkignoreIndicatorExists(indicatorData: any[], dataResult: any[]) {
        const ignoreIndicatorConstant = [5,6,7,8,11,15,17,124,125,136,111,112,113,114,115,116,117,118,119,120];

        let ignoreIndicatorData = indicatorData.filter(x => ignoreIndicatorConstant.includes(x.entityDynamicColumnId));
        
        ignoreIndicatorData.map(indicatorData => {
            let ignoredRecordData = dataResult.find(x => x.columnId === indicatorData.id);
            var index = dataResult.indexOf(ignoredRecordData);
            if (index !== -1) {
                dataResult.splice(index, 1);
            }
        });
    }

    isRecordCollectedForFacility(facilityUniqueId, instanceId) {
        return new Promise<boolean>((resolve, reject) => {
            this.facilityRecordsDbService.getAllFacilityRecords().subscribe((data) => {
                let dataResult = data.filter(x => x.instanceId == instanceId && x.facilityId == facilityUniqueId);

                this.facilityIndicatorDbService.getFacilityIndicatorsByInstanceId(instanceId).subscribe((facilityIndicatorData) => {
                    this.checkignoreIndicatorExists(facilityIndicatorData, dataResult);
                    let recordStatus;
                    if (dataResult.length <= 0) { recordStatus = false; }
                    else {
                        recordStatus = dataResult.find(x => x.status !== CollectionStatus.Collected) ? false : true;
                    }

                    resolve(recordStatus);
                });
            });
        });
    }

    isRecordCollectedForBeneficiary(beneficiaryUniqueId, instanceId) {
        return new Promise<boolean>((resolve, reject) => {
            this.beneficiaryRecordsDbService.getAllBeneficiaryRecords().subscribe((data) => {
                var dataResult = data.filter(x => x.instanceId == instanceId && x.beneficiaryId == beneficiaryUniqueId);
                this.beneficiaryIndicatorDbService.getBeneficiaryIndicatorsByInstanceId(instanceId).subscribe((beneficiaryIndicatorData) => {

                    this.checkignoreIndicatorExists(beneficiaryIndicatorData, dataResult);
                    let recordStatus;
                    if (dataResult.length <= 0) { recordStatus = false; }
                    else {
                        recordStatus = dataResult.find(x => x.status !== CollectionStatus.Collected) ? false : true;
                    }

                    resolve(recordStatus);
                });
            });
        });
    }
}