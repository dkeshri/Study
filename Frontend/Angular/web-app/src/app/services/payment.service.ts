import { Injectable } from '@angular/core';
import { ApiService } from '../core/services/api/api.service';
import { Observable } from 'rxjs';
import { Payment } from './models/payment.model';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  constructor(private apiService: ApiService) { }

  getPaymentsByCustomerId(customerId:number): Observable<Payment[]> {
    let params = new HttpParams();
    params = params.append('customerId', customerId);
    return this.apiService.get<Payment[]>('Payments',params);
  }
  getPaymentDetailByInvoiceId(invoiceId:string):Observable<Payment>{
    return this.apiService.get<Payment>('Payments/invoices/'+invoiceId)
  }
}
