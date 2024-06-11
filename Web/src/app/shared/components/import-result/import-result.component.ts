import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PaginationInstance } from 'ngx-pagination';
import { IImportResult } from 'src/app/models/import/import-result.model';

@Component({
    templateUrl: "./import-result.component.html"
})
export class IMportResultComponent implements OnInit{
   
    
  public pagingConfig: PaginationInstance  = {
    id: 'import_result_list',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
      };
  
    public importResult: IImportResult;
    constructor(private activeModal: NgbActiveModal){

    }

    close(){
        this.activeModal.close();
    }
    handleCancel(){
        this.close();
    }
    handleOk(){
        this.close();
    }

    ngOnInit(): void {
        this.pagingConfig.totalItems = this.importResult.rowErrors.length;
    }

    getPage(pageNo: number){
        this.pagingConfig.currentPage = pageNo;
    }
    
    copyToClipboard(){
        let textarea = document.createElement('textarea');        
        textarea.textContent= "SL\tRow Number\tError Detail\n";
        this.importResult.rowErrors.forEach( (item, index)=> {
            textarea.textContent += `${index+1}\t${item.rowNumber}\t${item.errorMessage}\n`;            
        });                 
        document.body.appendChild(textarea);
        textarea.select();
        document.execCommand('copy');  
        document.body.removeChild(textarea);
    }
}