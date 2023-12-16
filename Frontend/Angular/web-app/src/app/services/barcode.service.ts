import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Barcode, BarcodeCreate } from '../core/model/barcode-model';
import { ApiService } from '../core/services/api/api.service';

@Injectable({
  providedIn: 'root'
})
export class BarcodeService {

  constructor(private apiService: ApiService) { }
  getBarcode(req:BarcodeCreate):Observable<Barcode>{
    return this.apiService.post<Barcode>('barcode',req);
  }
}
