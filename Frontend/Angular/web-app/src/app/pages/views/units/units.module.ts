import { SharedModule } from './../../../shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UnitsRoutingModule } from './units-routing.module';
import { UnitDetailsComponent } from './unit-details/unit-details.component';
import { UnitsComponent } from './units/units.component';
import { CreateUnitComponent } from './create-unit/create-unit.component';
import { CreateUnitConversionComponent } from './create-unit-conversion/create-unit-conversion.component';


@NgModule({
  declarations: [
    UnitsComponent,
    UnitDetailsComponent,
    CreateUnitComponent,
    CreateUnitConversionComponent
  ],
  imports: [
    UnitsRoutingModule,
    SharedModule
  ]
})
export class UnitsModule { }
