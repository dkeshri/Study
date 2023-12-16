import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Arrear, CreateCustomer, Customer } from '../models/customer.model';
import { ApiService } from 'src/app/core/services/api/api.service';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  constructor(private apiService: ApiService) { }
  getCustomers(): Observable<Customer[]> {
    return this.apiService.get<Customer[]>('Customers');
  }
  createCustomer(createCustomer:CreateCustomer):Observable<Customer>{
    return this.apiService.post<Customer>('Customers',createCustomer);
  }
  getCustomerById(customerId:string):Observable<Customer>{
    return this.apiService.get<Customer>('Customers/'+customerId)
  }
  getCustomerByBarcode(barcode:string):Observable<Customer>{
    return this.apiService.get<Customer>('Customers/barcode/'+barcode)
  }
  updateCustomer(customer:Customer):Observable<Customer>{
    return this.apiService.put<Customer>('Customers/'+customer.id,customer);
  }
  deleteCustomer(id:string):Observable<Customer>{
    return this.apiService.delete<Customer>('Customers/'+id);
  }
  getCustomerArrearByCustomerId(customerId:string):Observable<Arrear>{
    return this.apiService.get<Arrear>('Customers/'+customerId+'/arrears')
  }
}
