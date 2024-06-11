import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ColumnDataTypeComponent } from './column-data-type.component';

describe('ColumnDataTypeComponent', () => {
  let component: ColumnDataTypeComponent;
  let fixture: ComponentFixture<ColumnDataTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColumnDataTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColumnDataTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
