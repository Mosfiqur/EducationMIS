import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { LoadingSpinnerService } from './loading-spinner.service';


@Component({
  selector: 'app-loading-spinner',
  templateUrl: './loading-spinner.component.html',
  styleUrls: ['./loading-spinner.component.scss'],
})
export class LoadingSpinnerComponent implements OnInit {

  loading: boolean;

  constructor(private loadingSpinnerService: LoadingSpinnerService) {

    this.loadingSpinnerService.isLoading.subscribe((v) => {      
      this.loading = v;
    });
  }
  ngOnInit() {
  }

}