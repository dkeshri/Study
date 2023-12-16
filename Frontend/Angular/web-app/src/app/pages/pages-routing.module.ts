import { PageNotFoundComponent } from './../theme/components/page-not-found/page-not-found.component';
import { BaseLayoutComponent } from './../theme/components/base-layout/base-layout.component';

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path:'',
    component:BaseLayoutComponent,
    children:[
        {path:'views', loadChildren:()=>import('./views/views.module').then(module=>module.ViewsModule)},
        {path:'', redirectTo:'views',pathMatch:'full'}     
    ]
  },
  {
    path:'**',
    component:PageNotFoundComponent 
  } // must always be last.
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule { }
