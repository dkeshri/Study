import { Component, OnInit } from '@angular/core';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { AuthenticationService } from 'src/app/core/services/auth/authentication.service';
import { Shop } from 'src/app/services/models/shop.model';
import { ShopService } from 'src/app/services/shop.service';

@Component({
  selector: 'app-shop-list',
  templateUrl: './shop-list.component.html',
  styleUrls: ['./shop-list.component.css']
})
export class ShopListComponent implements OnInit {

  shops!: Shop[];
  constructor(private shopService: ShopService,
    private authenticationService : AuthenticationService,
    private dialogRef: DynamicDialogRef
    ) { }

  ngOnInit(): void {

    this.shopService.getShops().subscribe((shops) => {
      this.shops = shops;
    });
  }
  onCardClick(shopId:number,shopName:string){
    this.authenticationService.setSelectedShopId(shopId);
    this.authenticationService.setSelectedShopName(shopName);
    this.dialogRef?.close(shopName);
  }
}
