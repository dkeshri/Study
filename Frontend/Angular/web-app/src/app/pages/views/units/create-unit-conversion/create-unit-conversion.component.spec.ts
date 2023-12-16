import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUnitConversionComponent } from './create-unit-conversion.component';

describe('CreateUnitConversionComponent', () => {
  let component: CreateUnitConversionComponent;
  let fixture: ComponentFixture<CreateUnitConversionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUnitConversionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateUnitConversionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
