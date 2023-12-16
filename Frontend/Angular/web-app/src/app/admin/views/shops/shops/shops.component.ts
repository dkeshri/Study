import { Component, OnInit } from '@angular/core';
import { Shop } from 'src/app/services/models/shop.model';
import { ShopService } from 'src/app/services/shop.service';
import { ColumnType, TableColumn } from 'src/app/theme/models/table-model';

@Component({
  selector: 'app-shops',
  templateUrl: './shops.component.html',
  styleUrls: ['./shops.component.css']
})
export class ShopsComponent implements OnInit {

  shops!: Shop[];
  cols!: TableColumn[];
  isEditable:boolean = false;
  constructor(private shopService:ShopService) {
  }

  ngOnInit(): void {
    this.cols = [
      { field: 'id', header: 'Shop Id', sortable:true,filtrable:true ,type:ColumnType.Link},
      { field: 'name', header: 'Shop Name', sortable:true, filtrable:true},
      { field: 'phone', header: 'Phone No.', sortable:true,filtrable:true},
      { field: 'isActive', header: 'isActive', sortable:false ,filtrable:false},
      { field: 'userId', header: 'User Id', sortable:false ,filtrable:true,type:ColumnType.SubLink,route:'../users/'}
  ] as TableColumn[];
  this.shopService.getAllShops().subscribe((shops)=>{
    this.shops = shops;
  });
  }

}
