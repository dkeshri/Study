import { UnitDetailsComponent } from './unit-details/unit-details.component';
import { UnitsComponent } from './units/units.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path:'',
    children:[
      {
        path:'',
        component:UnitsComponent
      },
      {
        path:':unitId',
        component:UnitDetailsComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UnitsRoutingModule { }
