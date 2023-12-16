import { PageNotFoundComponent } from './theme/components/page-not-found/page-not-found.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { RedirectToLandingPageComponent } from './theme/components/redirect-to-landing-page/redirect-to-landing-page.component';

const routes: Routes = [
  {
    path:'pages', loadChildren:()=>import('./pages/pages.module').then(module=>module.PagesModule),
    canActivate: [AuthGuard]
  },
  {
    path:'admin',loadChildren:()=>import('./admin/admin.module').then(module=>module.AdminModule),
    canActivate:[AuthGuard]
  },
  {
    path:'auth', loadChildren:()=>import('./auth/auth.module').then(module=>module.AuthModule)
  },
  {
    path:'',
    component:RedirectToLandingPageComponent,
    pathMatch:'full'
  },
  {
    path:'**',
    component:PageNotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes,{useHash:true})],
  exports: [RouterModule],
  providers: [AuthGuard]
})
export class AppRoutingModule { }
