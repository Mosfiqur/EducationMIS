import { LevelRank } from 'src/app/_enums/levelRank';
import { IPermission } from './permission.model';

export interface IRole{
    id?: number;
    roleName: string;
    levelRank: LevelRank
}

export class Role implements IRole{
    static createFrom(value?: Partial<Role>) {
      return Object.assign({}, value);
    }
    id?: number;
    roleName: string;
    levelRank: LevelRank;
    levelName?: string;
    levelId: number;
    isSelected: boolean;
    permissions?: IPermission[];
    permissionPresetId?: number;
    constructor(
        roleName: string, 
        levelRank: LevelRank,
        levelId: number,
        levelName?: string,
        id?: number,
        permissionPresetId?: number,
        permissions?: IPermission[],        
        ){
        this.id = id;
        this.roleName = roleName;
        this.levelRank = levelRank;
        this.levelId = levelId;
        this.levelName = levelName;
        this.isSelected = false;
        this.permissions = permissions || [];
        this.permissionPresetId = permissionPresetId;
    }
}