import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvoicesComponent } from './invoices/invoices.component';
import { InvoiceDetailComponent } from './invoice-detail/invoice-detail.component';

const routes: Routes = [
  {
    path:'',
    children:[
      {
        path:'',
        component:InvoicesComponent
      },
      {
        path:':invoiceId/:customerId',
        component:InvoiceDetailComponent
      },
      {
        path:':invoiceId',
        component:InvoiceDetailComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvoicesRoutingModule { }
