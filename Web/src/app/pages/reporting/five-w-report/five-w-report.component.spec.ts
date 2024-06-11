import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FiveWReportComponent } from './five-w-report.component';

describe('FiveWReportComponent', () => {
  let component: FiveWReportComponent;
  let fixture: ComponentFixture<FiveWReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FiveWReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FiveWReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
