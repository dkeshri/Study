import { Injectable } from '@angular/core';
import { ApiService } from '../core/services/api/api.service';
import { Observable, map } from 'rxjs';
import { City, Country, State } from './models/geo-region.model';
import { Option } from 'src/app/theme/models/table-model';
import { HttpParams } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class GeoRegionService {

  constructor(private apiService:ApiService) { }

  getCountries(): Observable<Country[]> {
    return this.apiService.get<Country[]>('Countries');
  }
  getCountriesOptions():Observable<Option[]>{
    return this.getCountries().pipe(map((countries : Country[])=>{
      let options:Option[];
      options = countries.map(element => {
        let option = {} as Option;
        option.value = element.iso2;
        option.label = element.name;
        return option;
      });
      return options;
    }));
  }
  getStates(): Observable<State[]> {
    return this.apiService.get<State[]>('States');
  }
  getStatesByCountryCode(countryCode:string): Observable<State[]> {
    let params = new HttpParams();
    params = params.append('countryCode', countryCode);
    return this.apiService.get<State[]>('States',params);
  }
  getStatesOptions():Observable<Option[]>{
    return this.getStates().pipe(map((states : State[])=>{
      let options:Option[];
      options = states.map(element => {
        let option = {} as Option;
        option.value = element.stateCode;
        option.label = element.name;
        return option;
      });
      return options;
    }));
  }
  getStatesOptionsByCountryCode(countryCode:string):Observable<Option[]>{
    return this.getStatesByCountryCode(countryCode).pipe(map((states : State[])=>{
      let options:Option[];
      options = states.map(element => {
        let option = {} as Option;
        option.value = element.stateCode;
        option.label = element.name;
        return option;
      });
      return options;
    }));
  }
  getCities(): Observable<City[]> {
    return this.apiService.get<City[]>('Cities');
  }
  getCitiesByCountryAndStateCode(countryCode:string, stateCode:string): Observable<City[]> {
    let params = new HttpParams();
    params = params.append('countryCode', countryCode);
    params = params.append('stateCode', stateCode);
    return this.apiService.get<City[]>('Cities',params);
  }
  getCitiesOptionsByCountryAndStateCode(countryCode:string, stateCode:string):Observable<Option[]>{
    return this.getCitiesByCountryAndStateCode(countryCode,stateCode).pipe(map((cities : City[])=>{
      let options:Option[];
      options = cities.map(element => {
        let option = {} as Option;
        option.value = element.name;
        option.label = element.name;
        return option;
      });
      return options;
    }));
  }
}
