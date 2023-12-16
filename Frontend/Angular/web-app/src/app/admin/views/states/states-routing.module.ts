import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StatesComponent } from './states/states.component';
import { StateDetailComponent } from './state-detail/state-detail.component';

const routes: Routes = [
  {
    path:'',
    children:[
      {
        path:'',
        component: StatesComponent
      },
      {
        path:':stateId',
        component:StateDetailComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StatesRoutingModule { }
