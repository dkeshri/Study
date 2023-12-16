import { Injectable } from '@angular/core';
import { ApiService } from '../core/services/api/api.service';
import { Observable, map } from 'rxjs';
import { UserRole } from './models/user.model';
import { Option } from '../theme/models/table-model';

@Injectable({
  providedIn: 'root'
})
export class UserRoleService {

  constructor(private apiService:ApiService) { }

  getUserRoles(): Observable<UserRole[]> {
    return this.apiService.get<UserRole[]>('Roles');
  }
  getUserRolesOptions():Observable<Option[]>{
    return this.getUserRoles().pipe(map((items)=>{
      let options:Option[];
      options = items.map(element => {
        let option = {} as Option;
        option.value = element.role;
        option.label = element.name;
        return option;
      });
      return options;
    }));
  }
}
