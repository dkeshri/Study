import { NgModule } from '@angular/core';

import { CitiesRoutingModule } from './cities-routing.module';
import { CitiesComponent } from './cities/cities.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { CityDetailComponent } from './city-detail/city-detail.component';


@NgModule({
  declarations: [
    CitiesComponent,
    CityDetailComponent
  ],
  imports: [
    SharedModule,
    CitiesRoutingModule
  ]
})
export class CitiesModule { }
