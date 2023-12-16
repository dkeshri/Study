import { AppConstants } from 'src/app/shared/models/app-constants';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { CoreModule } from './core/core.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { JsonAppConfigService } from './core/services/json-app-config.service';
import { AppConfig } from './core/model/app-config';

export function initializerFn(jsonAppConfigService: JsonAppConfigService) {
  return () => {
    return jsonAppConfigService.load();
  };
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CoreModule.forRoot(),
    TranslateModule.forRoot({
      defaultLanguage:localStorage.getItem(AppConstants.LANG_KEY)===null? 'en-US': JSON.parse(localStorage.getItem(AppConstants.LANG_KEY) || ''),
      loader:{
        provide: TranslateLoader,
        useFactory: (http:HttpClient) =>
        new TranslateHttpLoader(http, './assets/i18n/', '.json'),
        deps: [HttpClient]
      }
     }),
    AppRoutingModule
  ],
  providers: [
    {
      provide: AppConfig,
      deps: [HttpClient],
      useExisting: JsonAppConfigService
    },
    {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [JsonAppConfigService],
      useFactory: initializerFn
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
