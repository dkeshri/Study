import { map, Observable } from 'rxjs';
import { CreateUnit, Unit } from './models/unit.model';
import { ApiService } from './../core/services/api/api.service';
import { Injectable } from '@angular/core';
import { Option } from 'src/app/theme/models/table-model';
@Injectable({
  providedIn: 'root'
})
export class UnitService {

  constructor(private apiService:ApiService) { }

  getUnits():Observable<Unit[]>{
    return this.apiService.get<Unit[]>('units');
  }
  getUnit(id:string):Observable<Unit>{
    return this.apiService.get<Unit>('units/'+id);
  }
  getUnitOptions():Observable<Option[]>{
    return this.getUnits().pipe(map((units)=>{
      let options:Option[];
      options = units.map(element => {
        let option = {} as Option;
        option.value = element.id;
        option.label = element.shortName;
        return option;
      });
      return options;
    }));
  }
  createUnit(unit:CreateUnit):Observable<Unit>{
    return this.apiService.post<Unit>('Units',unit);
  }
}
