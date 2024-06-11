export class ListDataTypeViewModel {
    id?: number;
    name: string;
    listItems: ListItem[];    
}

export class ListItem{
    id: number;
    title: string;
    value: number;
    isSelected?: boolean;
}