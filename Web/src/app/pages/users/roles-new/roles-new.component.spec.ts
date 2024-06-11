import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RolesNewComponent } from './roles-new.component';

describe('RolesNewComponent', () => {
  let component: RolesNewComponent;
  let fixture: ComponentFixture<RolesNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RolesNewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RolesNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
