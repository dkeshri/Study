import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  get(key:string){
    let value = localStorage.getItem(key);
    if(value){
      return JSON.parse(value);
    }else{
      return null;
    }
  }
  set(key:string,value:any){
    localStorage.setItem(key,JSON.stringify(value));
    return true;
  }
  remove(key:string){
    localStorage.removeItem(key);
    return true;
  }
  clear(){
    localStorage.clear();
  }
}
