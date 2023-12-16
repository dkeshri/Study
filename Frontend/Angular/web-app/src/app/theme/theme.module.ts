import { PrimengModule } from './modules/primeng/primeng.module';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseLayoutComponent } from './components/base-layout/base-layout.component';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { LogoComponent } from './components/logo/logo.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MaterialModule } from './modules/material/material.module';
import { TranslateModule } from '@ngx-translate/core';
import { RedirectToLandingPageComponent } from './components/redirect-to-landing-page/redirect-to-landing-page.component';
const COMMON_MODULE = [
  CommonModule,
  FormsModule,
  ReactiveFormsModule,
  RouterModule,
  FontAwesomeModule,
  TranslateModule
]


@NgModule({
  declarations: [
    BaseLayoutComponent,
    FooterComponent,
    HeaderComponent,
    LogoComponent,
    PageNotFoundComponent,
    RedirectToLandingPageComponent
  ],
  imports: [
    ...COMMON_MODULE,
    PrimengModule,
    MaterialModule
    
  ],
  exports:[
    CommonModule,
    BaseLayoutComponent,
    PageNotFoundComponent,
    RedirectToLandingPageComponent,
    ...COMMON_MODULE,
    MaterialModule,
    PrimengModule
    
  ]
})
export class ThemeModule { }
