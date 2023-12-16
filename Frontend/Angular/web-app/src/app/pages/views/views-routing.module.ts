import { PageNotFoundComponent } from './../../theme/components/page-not-found/page-not-found.component';
import { AboutComponent } from './about/about.component';
import { HomeComponent } from './home/home.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'home',
        component: HomeComponent
      },
      {
        path: 'about',
        component: AboutComponent
      },
      {
        path: 'orders',
        loadChildren: () => import('./orders/orders.module').then(module => module.OrdersModule)
      },
      {
        path: 'customers',
        loadChildren: () => import('./customers/customers.module').then(module => module.CustomersModule)
      },
      {
        path: 'invoices',
        loadChildren: () => import('./invoices/invoices.module').then(module => module.InvoicesModule)
      },
      {
        path: 'products',
        loadChildren: () => import('./products/products.module').then(module => module.ProductsModule)
      },
      {
        path: 'units',
        loadChildren: () => import('./units/units.module').then(module => module.UnitsModule)
      },
      {
        path: 'payments',
        loadChildren: () => import('./payments/payments.module').then(module => module.PaymentsModule)
      },
      {
        path: 'profile',
        loadChildren: () => import('./profile/profile.module').then(module => module.ProfileModule)
      },
      {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
      }
    ]
  },
  {
    path: '**',
    component: PageNotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ViewsRoutingModule { }
