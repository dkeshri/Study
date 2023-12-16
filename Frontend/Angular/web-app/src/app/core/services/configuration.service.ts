import { Injectable } from '@angular/core';
import { AppConfig } from '../model/app-config';
@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {
  constructor(private appConfig: AppConfig) {
    
  }
  // public getConfig(key: string): any {
  //   return this.config[key];
  // }
  public getEndpoint():string{
    return this.appConfig.endPoints??'';
  }
  public getVersion():string{
    return this.appConfig.version??'0.0.0';
  }
}
