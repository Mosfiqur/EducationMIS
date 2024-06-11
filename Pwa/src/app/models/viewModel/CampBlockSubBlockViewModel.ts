import { Block, SubBlock, Camp } from '../idbmodels/indexedDBModels';

export class CampBlockSubBlockViewModel{
    camp:Camp;
    blocks: Block[];
    subBlocks:SubBlock[];
}