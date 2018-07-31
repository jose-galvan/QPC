import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QualityControlsComponent } from './quality-controls.component';

describe('QualityControlsComponent', () => {
  let component: QualityControlsComponent;
  let fixture: ComponentFixture<QualityControlsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QualityControlsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QualityControlsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
