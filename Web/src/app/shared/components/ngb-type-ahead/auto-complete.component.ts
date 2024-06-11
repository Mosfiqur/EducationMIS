import { Component, Input } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs/operators';
import { CommonService } from 'src/app/services/common.service';

@Component({
    selector: 'auto-complete',
    templateUrl: './auto-complete.component.html',    
    styles: [`.form-control { width: 300px; }`]
  })
  export class AutoCompleteComponent {    
    model: any;
    searching = false;
    searchFailed = false;
  
    constructor(private commonService: CommonService) {}
  
    search = (text$: Observable<string>) =>
      text$.pipe(
        debounceTime(300),
        distinctUntilChanged(),
        tap(() => this.searching = true),
        switchMap(term =>
          this.commonService.searchCamp(term).pipe(
            tap(() => this.searchFailed = false),
            catchError(() => {
              this.searchFailed = true;
              return of([]);
            }))
        ),
        tap(() => this.searching = false)
      )

      formatter = (x: {name: string}) => x.name;
  }
