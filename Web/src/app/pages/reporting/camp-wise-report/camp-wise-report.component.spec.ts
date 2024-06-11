import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CampWiseReportComponent } from './camp-wise-report.component';

describe('CampWiseReportComponent', () => {
  let component: CampWiseReportComponent;
  let fixture: ComponentFixture<CampWiseReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CampWiseReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CampWiseReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
