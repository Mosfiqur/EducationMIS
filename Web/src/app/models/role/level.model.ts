import { LevelRank } from 'src/app/_enums/levelRank';

export interface ILevel {    
    id: number,
    levelName: string;
    rank: LevelRank;
}