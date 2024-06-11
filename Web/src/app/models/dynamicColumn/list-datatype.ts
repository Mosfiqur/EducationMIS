import { ListItem } from './listItem';

export interface IListDataType {
    id?: number;
    name: string;
    listItems: ListItem[];    
}