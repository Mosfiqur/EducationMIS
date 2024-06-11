import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeIndicatorsOrderComponent } from './change-indicators-order.component';

describe('ChangeIndicatorsOrderComponent', () => {
  let component: ChangeIndicatorsOrderComponent;
  let fixture: ComponentFixture<ChangeIndicatorsOrderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChangeIndicatorsOrderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeIndicatorsOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
