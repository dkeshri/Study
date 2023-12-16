import { Component, OnInit } from '@angular/core';
import { DialogService } from 'primeng/dynamicdialog';
import { AuthenticationService } from 'src/app/core/services/auth/authentication.service';
import { ShopListComponent } from '../shop-list/shop-list.component';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  barcode:string = '';
  constructor( private router:Router,
    private activatedRoute: ActivatedRoute) {

  }

  ngOnInit(): void {

  }
  onEnterPress($event:any){
    let barcode = this.barcode;
    this.router.navigate(
      ['../orders/order'],
      {queryParams: { barcode: barcode }, relativeTo: this.activatedRoute}
      );
    console.log(barcode);
  }

}
