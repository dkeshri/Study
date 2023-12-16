import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/core/services/api/api.service';
import { UnitConversion } from '../Models/unit-conversion.model';
import { Observable, map } from 'rxjs';
import { Option } from 'src/app/theme/models/table-model';
@Injectable({
  providedIn: 'root'
})
export class UnitConversionService {

  constructor(private apiService:ApiService) { }

  getUnitConversions():Observable<UnitConversion[]>{
    return this.apiService.get<UnitConversion[]>('unitConversions');
  }
  createUnitConversion(createUnitConversion:UnitConversion):Observable<UnitConversion>{
    return this.apiService.post<UnitConversion>('unitConversions',createUnitConversion);
  }
  getUnitConversionOptions():Observable<Option[]>{
    return this.getUnitConversions().pipe(map((unitConversions)=>{
      let options:Option[];
      options = unitConversions.map(element => {
        let option = {} as Option;
        option.value = element.id;
        option.label = element.label;
        return option;
      });
      return options;
    }));
  }
}
