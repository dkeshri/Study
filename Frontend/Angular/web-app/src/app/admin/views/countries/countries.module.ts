import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CountriesRoutingModule } from './countries-routing.module';
import { CountriesComponent } from './countries/countries.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { CountryDetailComponent } from './country-detail/country-detail.component';


@NgModule({
  declarations: [
    CountriesComponent,
    CountryDetailComponent
  ],
  imports: [
    SharedModule,
    CountriesRoutingModule
  ]
})
export class CountriesModule { }
