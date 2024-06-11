import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { CommonEndpoints } from '../utility/endpoints/common';
import { IBaseQueryModel } from '../models/queryModels/base-query.model';
import { CampQueryModel } from '../models/queryModels/camp-query.model';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { Camp } from '../models/common/camp.model';
import { ApiConstant } from '../utility/ApiConstant';
import { BlockQueryModel } from '../models/queryModels/block-query.model';
import { SubBlockQueryModel } from '../models/queryModels/subBlock-query.model';
import { UnionQueryModel } from '../models/queryModels/union-query.model';
import { UpazilaQueryModel } from '../models/queryModels/upazila-query.model';
import { UnionViewModel } from '../models/common/unionViewModel';
import { UpazilaViewModel } from '../models/common/upazilaViewModel';
import { SubBlockViewModel } from '../models/common/subBlockViewModel';
import { BlockViewModel } from '../models/common/blockViewModel';
import { Globals } from '../globals';
import { IReportingFrequency } from '../models/frameworks/reporting-frequency.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { IAgeGroup } from '../models/frameworks/age-group.model';

@Injectable({
    providedIn: 'root'
})
export class CommonService {
    
    constructor(
        private httpClientService: HttpClientService,
        private endpoints : CommonEndpoints,
        private globals: Globals,
        private httpClient: HttpClient
        ){

    }

    searchCamp(key: string) : Observable<Camp[]> {
        let api = `${this.endpoints.getCamps}?pageNo=1&pageSize=${this.globals.maxPageSize}&searchText=${key}`;
        return this.httpClient.get<IPagedResponse<Camp>>(api)
        .pipe(
            map((res: IPagedResponse<Camp>) => res.data)
        );
    }

    async getAllCamps(): Promise<IPagedResponse<Camp>>{
        return this.httpClientService.getAsync(`
            ${this.endpoints.getCamps}?pageNo=${1}&pageSize=${this.globals.maxPageSize}`);            
    }

    async getCamps(query: CampQueryModel): Promise<IPagedResponse<Camp>>{
        return this.httpClientService.getAsync(`
            ${this.endpoints.getCamps}?pageNo=${query.pageNo}&pageSize=${query.pageSize}&unionId=${query.unionId ? query.unionId : ""}&searchText=${query.searchText ? query.searchText: ""}
            `);            
    }

    async getPaginatedCamps(query: IBaseQueryModel): Promise<IPagedResponse<Camp>>{
        return this.httpClientService.getAsync(`
            ${this.endpoints.getCamps}?pageNo=${query.pageNo}&pageSize=${query.pageSize}&searchText=${query.searchText ? query.searchText: ""}
            `);            
    }

    
    async getBlocks(query: BlockQueryModel): Promise<IPagedResponse<BlockViewModel>>{
        return this.httpClientService.getAsync(`
            ${ApiConstant.GetBlocks}?pageNo=${query.pageNo}&pageSize=${query.pageSize}&campId=${query.campId ? query.campId : ""}&searchText=${query.searchText ? query.searchText: ""}
            `);            
    }
    async getSubBlocks(query: SubBlockQueryModel): Promise<IPagedResponse<SubBlockViewModel>>{
        return this.httpClientService.getAsync(`
            ${ApiConstant.GetSubBlocks}?pageNo=${query.pageNo}&pageSize=${query.pageSize}&blockId=${query.blockId ? query.blockId : ""}&searchText=${query.searchText ? query.searchText: ""}
            `);            
    }
    async getUnions(query: UnionQueryModel): Promise<IPagedResponse<UnionViewModel>>{
        return this.httpClientService.getAsync(`
            ${ApiConstant.GetUnions}?pageNo=${query.pageNo}&pageSize=${query.pageSize}&upazilaId=${query.upazilaId ? query.upazilaId : ""}&searchText=${query.searchText ? query.searchText: ""}
            `);            
    }
    async getUpazilas(query: UpazilaQueryModel): Promise<IPagedResponse<UpazilaViewModel>>{
        return this.httpClientService.getAsync(`
            ${ApiConstant.GetUpazilas}?pageNo=${query.pageNo}&pageSize=${query.pageSize}&districtId=${query.districtId ? query.districtId : ""}&searchText=${query.searchText ? query.searchText: ""}
            `);            
    }

    async getAllReportingFrequencies(): Promise<IPagedResponse<IReportingFrequency>>{
        return this.httpClientService.getAsync(`
            ${this.endpoints.getReportingFrequencies}?pageNo=${1}&pageSize=${this.globals.maxPageSize}`);            
    }

    async getAllAgeGroups(): Promise<IPagedResponse<IAgeGroup>>{
        return this.httpClientService.getAsync(`
            ${this.endpoints.getAgeGroups}?pageNo=${1}&pageSize=${this.globals.maxPageSize}`);
    }
}