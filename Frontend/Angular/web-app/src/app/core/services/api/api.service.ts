import { map, Observable, catchError, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private httpClient:HttpClient) { }

  get<T>(path: string,params?:any): Observable<T>{
    return this.httpClient.get<T>(`${path}`,{observe:'body',params});
  }
  post<T>(path:string,body?:any): Observable<T>{
    return this.httpClient.post<T>(`${path}`,body);
  }
  delete<T>(path:string):Observable<T>{
    return this.httpClient.delete<T>(`${path}`)
  }
  put<T>(path:string,body?:any): Observable<T>{
    return this.httpClient.put<T>(`${path}`,body);
  }
}
