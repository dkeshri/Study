import { Observable } from 'rxjs';
import { ApiService } from './../core/services/api/api.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor(private apiService:ApiService) { }

  getHome():Observable<any>{
    return this.apiService.get<any>('Users',null);
  }
}
