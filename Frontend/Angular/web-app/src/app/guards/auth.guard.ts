import { AppConstants } from './../shared/models/app-constants';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../core/services/auth/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private authentivationService:AuthenticationService
    )
    {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      let isAuthenticated = this.authentivationService.isAuthenticated();
      if(!isAuthenticated){
        this.router.navigate([AppConstants.LOGIN_URL])
      }
      return isAuthenticated;
  }
  
}
