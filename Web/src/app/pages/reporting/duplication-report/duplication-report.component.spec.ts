import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DuplicationReportComponent } from './duplication-report.component';

describe('DuplicationReportComponent', () => {
  let component: DuplicationReportComponent;
  let fixture: ComponentFixture<DuplicationReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DuplicationReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DuplicationReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
