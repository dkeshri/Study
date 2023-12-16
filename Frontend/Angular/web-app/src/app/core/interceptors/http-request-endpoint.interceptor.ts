import { ConfigurationService } from './../services/configuration.service';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class HttpRequestEndpointInterceptor implements HttpInterceptor {

  constructor(private configurationService:ConfigurationService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let endPoint = this.configurationService.getEndpoint();
    if(request.url.indexOf('assets/')===-1){
      request = request.clone({
        url: endPoint+request.url
      });
    }
    return next.handle(request);
  }
}
