import { SharedModule } from './../../shared/shared.module';
import { NgModule } from '@angular/core';
import { ViewsRoutingModule } from './views-routing.module';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { ShopListComponent } from './shop-list/shop-list.component';


@NgModule({
  declarations: [
    HomeComponent,
    AboutComponent,
    ShopListComponent
  ],
  imports: [
    ViewsRoutingModule,
    SharedModule
  ]
})
export class ViewsModule { }
