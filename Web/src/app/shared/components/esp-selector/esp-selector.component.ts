import { Component, OnInit } from "@angular/core";
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { EducationSectorPartner } from 'src/app/models/educationSectorPartner/educationSectorPartner';
import { IEducationSectorPartner } from 'src/app/models/user/educationSectorPartner';
import { EducationPartnerService } from 'src/app/services/educationPartner.service';

@Component({
    templateUrl: './esp-selector.component.html',
    styleUrls: ['./esp-selector.component.scss']
})
export class EspSelectorComponent implements OnInit{


    public modalTitle: string;
    public fieldNameLabel: string;        
    public selectedEsp: IEducationSectorPartner;

    private _searchText: string;
    private _filteredEspList: EducationSectorPartner[];
    public espList: EducationSectorPartner[];

    public selectedPpId: number;
    public selectedTempPpId: number;
    public selectedTempPpName: string;

    public ppConfig = {
        itemsPerPage: 10,
        currentPage: 1,
        total: 0,
        searchText: "",
        id: 'programmingPartner_pagination'
      };


  get searchText(): string {
    return this._searchText;
  }

  set searchText(val: string) {        
    this._searchText = val;

    this._filteredEspList = this.getFilteredEsp(val);
  }

  get filteredEspList(): IEducationSectorPartner[]{
    return this._filteredEspList;
  }

  getFilteredEsp(searchText: string){
    if(!searchText) {
      return this.espList;
    }     
    return this.espList.filter(x=> x.partnerName.toLowerCase().includes(searchText.toLowerCase()));
  }

  constructor(
    private educationPartnerService: EducationPartnerService,
    private activeModal: NgbActiveModal
    ){

  }
  ngOnInit(): void {
    this.loadESP();
    
  }

  onPageChanged(pageNo: number){
    this.ppConfig.currentPage = pageNo;
  }

  loadESP() {
      this.educationPartnerService.getAll().then(data => {
        this.espList = data;    
        this._filteredEspList = this.espList;      
      });
  }

  ppRadioOnChange(pp) {
      this.selectedEsp = pp;      
  }


  onCancel(){
    this.activeModal.close();
  }

  onSubmit(){    
    this.activeModal.close(this.selectedEsp);
  }
}