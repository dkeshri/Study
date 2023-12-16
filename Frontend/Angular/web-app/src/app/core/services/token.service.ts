import { AppConstants, CustomHeader } from './../../shared/models/app-constants';
import { LocalStorageService } from './storages/local-storage.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private localStorageService: LocalStorageService) { }
  getToken(){
    let userData = this.localStorageService.get(AppConstants.AUTH_USER_KEY);
    if(userData !== null && userData.hasOwnProperty('token')){
      return userData['token'];
    }
    return null;

  }
  getUser(){
    let userData = this.localStorageService.get(AppConstants.AUTH_USER_KEY);
    if(userData !== null && userData.hasOwnProperty('userName')){
      return userData['userName'];
    }
    return null;
  }
  setToken(token:any){
    this.localStorageService.set(AppConstants.AUTH_USER_KEY,token);
  }
  logout(){
    this.localStorageService.remove(AppConstants.AUTH_USER_KEY);
    this.localStorageService.remove(CustomHeader.SHOP_ID);
    this.localStorageService.remove(AppConstants.SHOP_NAME);
  }

  getSelectedShopId(){
      let shopId = this.localStorageService.get(CustomHeader.SHOP_ID);
      return shopId;
  }

  setSelectedShopId(shopId:number){
    this.localStorageService.set(CustomHeader.SHOP_ID, shopId);
  }
  removeSelectedShopId(){
    this.localStorageService.remove(CustomHeader.SHOP_ID);
  }
  getShopName(){
    return this.localStorageService.get(AppConstants.SHOP_NAME);
  }
  setShopName(name:String){
    this.localStorageService.set(AppConstants.SHOP_NAME,name);
  }
}
