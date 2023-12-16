import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomersComponent } from './customers/customers.component';
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';
import { CustomerOrdersComponent } from './customer-orders/customer-orders.component';

const routes: Routes = [
  {
    path:'',
    children:[
      {
        path:'',
        component: CustomersComponent
      },
      {
        path:':customerId',
        component:CustomerDetailComponent
      },
      {
        path:':customerId/orders',
        component:CustomerOrdersComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomersRoutingModule { }
