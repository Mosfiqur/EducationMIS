import { Component, OnInit, ViewChild } from '@angular/core';

@Component({
    selector: 'ngx-list-type-cell-renderer',
    template: `
    <ng-template #listtemplate let-column="column" let-row="row" ngx-datatable-cell-template let-value="value">
       {{getData(value,column)}}
    </ng-template>
    `,
    styles: [

    ],
})

export class NgxListTypeCellRenderer {
    
    getData(row, col) {
        var column = col.cellEditorParams;
        console.log('data', row, column)
        let returnText = "";
        for (let i = 0; i < row.length; i++) {
            returnText = returnText + column.filter(a => a.id == row[i])[0].title + ':' + row[i] + ','
        }
        return returnText;//`Name ${row["name"]}, Gender is : ${row["company"]}`;
    }

}
