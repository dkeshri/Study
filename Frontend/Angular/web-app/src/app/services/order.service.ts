import { Injectable } from '@angular/core';
import { ApiService } from '../core/services/api/api.service';
import { Order, OrdersSummary } from '../pages/views/orders/Model/order-model';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private apiService: ApiService) { }

  createOrdersSummary(createOrdersSummary:OrdersSummary):Observable<OrdersSummary>{
    return this.apiService.post<OrdersSummary>('OrdersSummaries',createOrdersSummary);
  }
  getOrdersSummaries(): Observable<OrdersSummary[]> {
    return this.apiService.get<OrdersSummary[]>('OrdersSummaries');
  }
  getOrdersSummariesByCustomerId(customerId:number): Observable<OrdersSummary[]> {
    return this.apiService.get<OrdersSummary[]>('OrdersSummaries/Customers/'+customerId);
  }
  getOrdersByOrderSummaryId(orderSummaryId:number){
    return this.apiService.get<Order[]>('Orders/'+orderSummaryId);
  }
  getOrdersSummariesByFromToDate(customerId:number,from :Date,to:Date): Observable<OrdersSummary[]> {
    let params = new HttpParams();
    params = params.append('from', from.toISOString());
    params = params.append('to', to.toISOString());
    return this.apiService.get<OrdersSummary[]>('OrdersSummaries/Customers/'+customerId+'/filters',params);
  }
  getOrdersSummaryById(orderSummaryId:string){
    return this.apiService.get<OrdersSummary>('OrdersSummaries/'+orderSummaryId);
  }
}
