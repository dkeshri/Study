import { CreateProduct } from './../Models/product.model';
import { map, Observable } from 'rxjs';
import { ApiService } from './../../../../core/services/api/api.service';
import { Injectable } from '@angular/core';
import { Product } from '../Models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private apiService: ApiService) { }
  getProducts(): Observable<Product[]> {
    return this.apiService.get<Product[]>('Products');
  }
  getProduct(id:string): Observable<Product>{
    return this.apiService.get<Product>('Products/'+id);
  }
  getProductByBarcode(barcode:string): Observable<Product>{
    return this.apiService.get<Product>('Products/barcode/'+barcode);
  }
  createProduct(createProduct:Product):Observable<Product>{
    return this.apiService.post<Product>('Products',createProduct);
  }
  deleteProduct(id:string):Observable<Product>{
    return this.apiService.delete<Product>('Products/'+id);
  }
  updateProduct(product:Product):Observable<Product>{
    return this.apiService.put<Product>('Products/'+product.id,product);
  }
}
