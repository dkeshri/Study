
import { ThemeModule } from './../theme/theme.module';
import { NgModule } from '@angular/core';
import { DataTableComponent } from './components/data-table/data-table.component';
import { AddressComponent } from './components/address/address.component';
import { ActiveInactivePipe } from './pipes/active-inactive.pipe';
import { BarcodeInputComponent } from './components/barcode-input/barcode-input.component';
import { DatetimePipe } from './datetime.pipe';



@NgModule({
  declarations: [
    DataTableComponent,
    AddressComponent,
    ActiveInactivePipe,
    BarcodeInputComponent,
    DatetimePipe
  ],
  imports: [
    ThemeModule
  ],
  exports:[
    DataTableComponent,
    AddressComponent,
    BarcodeInputComponent,
    ThemeModule,
    ActiveInactivePipe,
    DatetimePipe
  ]
})
export class SharedModule { }
