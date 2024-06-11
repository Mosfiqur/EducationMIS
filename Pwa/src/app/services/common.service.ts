import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { Camp, Block, SubBlock } from '../models/idbmodels/indexedDBModels';
import { ApiConstant } from '../utility/ApiConstant';
import { CampQueryModel } from '../models/queryModels/camp-query.model';
import { BlockQueryModel } from '../models/queryModels/block-query.model';
import { SubBlockQueryModel } from '../models/queryModels/subBlock-query.model';
import {BaseQueryModel} from '../models/queryModels/baseQueryModel';
import { CampBlockSubBlockViewModel } from '../models/viewModel/CampBlockSubBlockViewModel';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(private httpClientService: HttpClientService) { }

  async getCampWithBlockSubBlock(query: BaseQueryModel): Promise<IPagedResponse<CampBlockSubBlockViewModel>>{
    return this.httpClientService.getAsync(`
        ${ApiConstant.GetCampWithBlockSubBlock}?pageNo=${query.pageNo}&pageSize=${query.pageSize}&searchText=${query.searchText ? query.searchText: ""}
        `);            
  }

  async getCamps(query: CampQueryModel): Promise<IPagedResponse<Camp>>{
    return this.httpClientService.getAsync(`
        ${ApiConstant.GetCamps}?pageNo=${query.pageNo}&pageSize=${query.pageSize}&unionId=${query.unionId ? query.unionId : ""}&searchText=${query.searchText ? query.searchText: ""}
        `);            
}
async getBlocks(query: BlockQueryModel): Promise<IPagedResponse<Block>>{
    return this.httpClientService.getAsync(`
        ${ApiConstant.GetBlocks}?pageNo=${query.pageNo}&pageSize=${query.pageSize}&campId=${query.campId ? query.campId : ""}&searchText=${query.searchText ? query.searchText: ""}
        `);            
}
async getSubBlocks(query: SubBlockQueryModel): Promise<IPagedResponse<SubBlock>>{
    return this.httpClientService.getAsync(`
        ${ApiConstant.GetSubBlocks}?pageNo=${query.pageNo}&pageSize=${query.pageSize}&blockId=${query.blockId ? query.blockId : ""}&searchText=${query.searchText ? query.searchText: ""}
        `);            
}
}
