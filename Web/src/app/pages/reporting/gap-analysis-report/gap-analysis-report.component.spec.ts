import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GapAnalysisReportComponent } from './gap-analysis-report.component';

describe('GapAnalysisReportComponent', () => {
  let component: GapAnalysisReportComponent;
  let fixture: ComponentFixture<GapAnalysisReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GapAnalysisReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GapAnalysisReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
