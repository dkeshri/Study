import { Observable, map } from 'rxjs';
import { ApiService } from './../api/api.service';
import { TokenService } from './../token.service';
import { Injectable } from '@angular/core';
import { AuthenticatedUser } from '../../model/authenticated-user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private apiService: ApiService,
    private tokenService: TokenService
  ) { }

  isAuthenticated() {
    return this.tokenService.getToken() != null;
  }

  logout() {
    this.tokenService.logout();
  }

  getUserSelectedShopId() {
    let shopId = this.tokenService.getSelectedShopId();
    return shopId;
  }
  getUserShopName(){
    return this.tokenService.getShopName();
  }
  getLoggedInUser(){
    return this.tokenService.getUser();
  }
  setSelectedShopName(name:string){
    this.tokenService.setShopName(name);
  }
  setSelectedShopId(shopId: number) {
    this.tokenService.setSelectedShopId(shopId);
  }
  removeSelectedShopId(){
    this.tokenService.removeSelectedShopId();
  }
  login(userCrediential: any): Observable<boolean> {
    return this.apiService.post<AuthenticatedUser>('auth/login', userCrediential).pipe(
      map((response) => {
        this.tokenService.setToken(response);
        if (response.shopIds?.length == 1) {
          this.setSelectedShopId(response.shopIds[0]);
        }else{
          this.setSelectedShopId(0);
        }
        return true;
      })
    );
  }
}