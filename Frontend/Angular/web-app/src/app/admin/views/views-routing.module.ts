import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PageNotFoundComponent } from 'src/app/theme/components/page-not-found/page-not-found.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'home',
        component: HomeComponent
      },
      {
        path: 'users',
        loadChildren: () => import('./users/users.module').then(module => module.UsersModule)

      },
      {
        path: 'shops',
        loadChildren: () => import('./shops/shops.module').then(module => module.ShopsModule)

      },
      {
        path: 'countries',
        loadChildren: () => import('./countries/countries.module').then(module => module.CountriesModule)

      },
      {
        path: 'states',
        loadChildren: () => import('./states/states.module').then(module => module.StatesModule)

      },
      {
        path: 'cities',
        loadChildren: () => import('./cities/cities.module').then(module => module.CitiesModule)

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
