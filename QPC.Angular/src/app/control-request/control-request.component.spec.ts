import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlRequestComponent } from './control-request.component';

describe('ControlRequestComponent', () => {
  let component: ControlRequestComponent;
  let fixture: ComponentFixture<ControlRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
