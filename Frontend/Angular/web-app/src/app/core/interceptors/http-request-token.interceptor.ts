import { TokenService } from './../services/token.service';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable()
export class HttpRequestTokenInterceptor implements HttpInterceptor {

  private openUrls: string[] = ['assets', 'login'];

  constructor(private tokenService: TokenService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let isValidRequest = true;
    this.openUrls.forEach(openUrl => {
      isValidRequest = isValidRequest && (request.url.indexOf(openUrl) === -1);
    });
    if (isValidRequest) {
      const token = this.tokenService.getToken();
      if (token) {
        request = request.clone({
          headers: request.headers.set('Authorization', 'Bearer ' + token)
        });
      }
    }
    return next.handle(request);
  }
  
}