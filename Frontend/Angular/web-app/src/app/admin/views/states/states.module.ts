import { NgModule } from '@angular/core';

import { StatesRoutingModule } from './states-routing.module';
import { StatesComponent } from './states/states.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { StateDetailComponent } from './state-detail/state-detail.component';


@NgModule({
  declarations: [
    StatesComponent,
    StateDetailComponent
  ],
  imports: [
    SharedModule,
    StatesRoutingModule
  ]
})
export class StatesModule { }
