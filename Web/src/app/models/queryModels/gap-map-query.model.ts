import { Gender } from 'src/app/_enums/gender';
import { IAgeGroup } from '../frameworks/age-group.model';

export interface IGapMapQueryModel {
    years: number[];
    genders: Gender[];
    ageGroupIds: number[];
    campIds: number[];
}