import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';

@Component({
    selector: 'page-size-selector',
    templateUrl: './page-size-selector.component.html'
})
export class PageSizeSelectorComponent implements OnChanges {

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.totalItems) {
            this.numberOfItemsShowing = changes.totalItems.currentValue || 0;
        }
        if (changes.size) {
            this._selectedPageSize = changes.size.currentValue || 10;
        }
    }

    @Output('onPageSizeChanged')
    pageSizeChangedEvent: EventEmitter<number> = new EventEmitter();

    @Input('totalItems') totalItems: number;
    @Input('size') size: number;

    public numberOfItemsShowing: number = 0;

    public pageSizes: ISelectListItem[] = [
        { id: 10, text: '10' },
        { id: 20, text: '20' },
        { id: 50, text: '50' },
        { id: 100, text: '100' },
        { id: 250, text: '250' },
    ]

    private _selectedPageSize: number = 10;

    get selectedPageSize(): number {
        return this._selectedPageSize;
    }
    set selectedPageSize(val: number) {
        this._selectedPageSize = val;
        this.onPageSizeChanged()
    }

    onPageSizeChanged() {
        this.pageSizeChangedEvent.emit(parseInt(this.selectedPageSize.toString()));
    }
}