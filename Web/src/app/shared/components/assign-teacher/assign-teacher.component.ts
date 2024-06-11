import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Globals } from 'src/app/globals';
import { TeacherViewModel } from 'src/app/models/user/teacherViewModel';
import { FacilityService } from 'src/app/services/facility.service';

@Component({
    templateUrl: './assign-teacher.component.html'
})
export class AssignTeacherComponent implements OnInit{
    public p: any;
    public pagingConfig = {
        id: 'teacher_pagination',
        itemsPerPage: 10,
        currentPage: 1,
        totalItems: 0,
        searchText: ""
    }

    private _userSearchText: string;
    private userSearchText$: Subject<string> = new Subject<string>();
    get userSearchText(): string {
       return this._userSearchText;
    }
    set userSearchText(val: string){
        this.userSearchText$.next(val);
    }

      
    public teachers: TeacherViewModel[];
    public selectedTeacher: TeacherViewModel = null;
    constructor(
        private facilityService: FacilityService,
        private activeModal: NgbActiveModal,
        private globals: Globals
        
        ){
            this.userSearchText$.pipe(
                debounceTime(this.globals.searchDebounce),
                distinctUntilChanged()
              ).subscribe(text => {
                this._userSearchText = text;
                this.pagingConfig.currentPage = 1;
                this.pagingConfig.searchText = text;
                this.getTeachers();
              }); 
    }

    ngOnInit(): void {    
        setTimeout(()=> {
            this.getTeachers();
        }, 0);
    }
    
    getTeachers(){
        this.facilityService.getTeachers(
            this.pagingConfig.itemsPerPage, 
            this.pagingConfig.currentPage, 
            this.pagingConfig.searchText
            ).then(data => {
            this.teachers = data.data;                        
            this.pagingConfig.totalItems = data.total;
        });
    }

    teacherRadioOnChange(teacher) {
        this.selectedTeacher = teacher;
    }

    getPage(pageNo: number){
        this.pagingConfig.currentPage = pageNo;
        this.getTeachers();
    }
    
    onCancel(){
        this.activeModal.close();
    }
    onSubmit(){
        if(!this.selectedTeacher){
            return;
        }
        this.activeModal.close(this.selectedTeacher);
    }
}
