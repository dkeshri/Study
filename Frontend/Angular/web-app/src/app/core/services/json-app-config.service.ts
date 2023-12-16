import { Injectable, isDevMode } from '@angular/core';
import { AppConfig } from '../model/app-config';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class JsonAppConfigService extends AppConfig{

  constructor(private http: HttpClient) {
    super();
  }
  load() {
    let configFile = "assets/config/config.json";
    if (isDevMode()) {
      configFile = "assets/config/config-dev.json";
    }
    return this.http.get<AppConfig>(configFile)
      .toPromise()
      .then(data => {
        this.endPoints = data?.endPoints;
        this.version = data?.version;
      })
      .catch(() => {
        console.error('Could not load configuration');
      });
  }
}
