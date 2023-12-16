import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { ApiService } from '../core/services/api/api.service';
import { User } from './models/user.model';
import { Role } from '../shared/models/app-constants';
import { Option } from '../theme/models/table-model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private apiService: ApiService) { }

  getUsers(): Observable<User[]> {
    return this.apiService.get<User[]>('Users');
  }
  getLoggedInUserDetail():Observable<User>{
    return this.apiService.get<User>('Users/user');
  }

  isSuperAdmin():Observable<boolean>{
    return this.getLoggedInUserDetail().pipe(map((user:User)=>{
      let isSuperAdmin:boolean = false;
      if(user.role===Role.SUPER_ADMIN){
        isSuperAdmin = true;
      }
      return isSuperAdmin;
    }));
  }
  getUser(id:string):Observable<User>{
    return this.apiService.get<User>('Users/'+id);
  }
  getUsersOptions():Observable<Option[]>{
    return this.getUsers().pipe(map((items)=>{
      let options:Option[];
      options = items.map(element => {
        let option = {} as Option;
        option.value = element.id;
        option.label = element.username;
        return option;
      });
      return options;
    }));
  }
  createUser(user:User):Observable<User>{
    return this.apiService.post<User>('Users',user);
  }
  deleteUser(id:string):Observable<User>{
    return this.apiService.delete<User>('Users/'+id);
  }
  updateUser(user:User):Observable<User>{
    return this.apiService.put<User>('Users/'+user.id,user);
  }
}
