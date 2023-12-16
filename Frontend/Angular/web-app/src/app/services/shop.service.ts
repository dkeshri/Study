import { Injectable } from '@angular/core';
import { ApiService } from '../core/services/api/api.service';
import { Observable, map } from 'rxjs';
import { Shop } from './models/shop.model';
import { Option } from 'src/app/theme/models/table-model';
import { RouteAction } from '../shared/models/app-constants';
@Injectable({
  providedIn: 'root'
})
export class ShopService {

  constructor(private apiService: ApiService) { }
  getShops(): Observable<Shop[]> {
    return this.apiService.get<Shop[]>('Shops');
  }

  getAllShops(): Observable<Shop[]> {
    return this.apiService.get<Shop[]>('Shops/'+RouteAction.ALL);
  }

  getShopOptions():Observable<Option[]>{
    return this.getShops().pipe(map((shops)=>{
      let options:Option[];
      options = shops.map(element => {
        let option = {} as Option;
        option.value = element.id;
        option.label = element.name;
        return option;
      });
      return options;
    }));
  }
  getShop(shopId:string):Observable<Shop>{
    return this.apiService.get<Shop>('Shops/'+shopId);
  }
  createShop(shop:Shop):Observable<Shop>{
    return this.apiService.post<Shop>('Shops',shop);
  }
  deleteShop(id:string):Observable<Shop>{
    return this.apiService.delete<Shop>('Shops/'+id);
  }
  updateShop(shop:Shop):Observable<Shop>{
    return this.apiService.put<Shop>('Shops/'+shop.id,shop);
  }

}
