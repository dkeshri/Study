import { ToastModule } from 'primeng/toast';
import { HttpRequestEndpointInterceptor } from './interceptors/http-request-endpoint.interceptor';
import { HttpRequestTokenInterceptor } from './interceptors/http-request-token.interceptor';
import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpErrorHandlerInterceptor } from './interceptors/http-error-handler.interceptor';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { HttpRequestHeaderInterceptor } from './interceptors/http-request-header.interceptor';
import { DatetimePipe } from '../shared/datetime.pipe';
import { NgxUiLoaderHttpModule, NgxUiLoaderModule } from 'ngx-ui-loader';
import { ngxUiLoaderConfig } from './model/loader-config';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HttpClientModule,
    ConfirmDialogModule,
    ToastModule,
    NgxUiLoaderModule.forRoot(ngxUiLoaderConfig),
    NgxUiLoaderHttpModule.forRoot({ showForeground: true })
  ],
  providers:[
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpRequestEndpointInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpRequestTokenInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpRequestHeaderInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorHandlerInterceptor,
      multi: true
    },
    MessageService,ConfirmationService
  ],
  exports:[
    HttpClientModule,
    ConfirmDialogModule,
    ToastModule,
    NgxUiLoaderModule,
    NgxUiLoaderHttpModule
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule){
    if(parentModule){
      throw new Error(`Core module is already loaded. Import core module only once in root module.`);
    }
  }
  static forRoot(): ModuleWithProviders<CoreModule>{
    return{
      ngModule: CoreModule,
      providers:[DatetimePipe]
    }
  }
}
