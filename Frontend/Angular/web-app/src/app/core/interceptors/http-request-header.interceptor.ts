import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { CustomHeader } from 'src/app/shared/models/app-constants';
import { AuthenticationService } from '../services/auth/authentication.service';
import { TokenService } from '../services/token.service';

@Injectable()
export class HttpRequestHeaderInterceptor implements HttpInterceptor {

  constructor(private authenticationService : AuthenticationService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let shopId = this.authenticationService.getUserSelectedShopId();
    if(shopId){
      request = request.clone({
        headers: request.headers.set(CustomHeader.SHOP_ID, JSON.stringify(shopId))
      });
    }
    return next.handle(request);
  }
}
