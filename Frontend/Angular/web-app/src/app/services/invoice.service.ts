import { Injectable } from '@angular/core';
import { ApiService } from '../core/services/api/api.service';
import { Observable } from 'rxjs';
import { Invoice } from './models/invoice.model';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {

  constructor(private apiService: ApiService) { }

  getInvoicesByCustomerId(customerId:number): Observable<Invoice[]> {
    let params = new HttpParams();
    params = params.append('customerId', customerId);
    return this.apiService.get<Invoice[]>('Invoices',params);
  }
  createInvoice(createInvoice:Invoice):Observable<Invoice>{
    return this.apiService.post<Invoice>('Invoices',createInvoice);
  }
  
  getInvoice(id:string): Observable<Invoice>{
    return this.apiService.get<Invoice>('Invoices/'+id);
  }
}
