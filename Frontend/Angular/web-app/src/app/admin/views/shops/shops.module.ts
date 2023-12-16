import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ShopsRoutingModule } from './shops-routing.module';
import { ShopsComponent } from './shops/shops.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ShopDetailComponent } from './shop-detail/shop-detail.component';


@NgModule({
  declarations: [
    ShopsComponent,
    ShopDetailComponent
  ],
  imports: [
    SharedModule,
    ShopsRoutingModule
  ]
})
export class ShopsModule { }
