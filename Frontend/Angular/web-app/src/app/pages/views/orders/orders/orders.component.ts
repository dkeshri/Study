import { ColumnType, ModifiedTableRow, Option } from './../../../../theme/models/table-model';
import { Component, OnInit } from '@angular/core';
import { OrderService } from 'src/app/services/order.service';
import { TableColumn } from 'src/app/theme/models/table-model';
import { OrdersSummary } from '../Model/order-model';
import { CustomerService } from '../../customers/services/customer.service';
import { Format } from 'src/app/shared/models/app-constants';
@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  barcode:string = '';
  ordersSummaries!: OrdersSummary[];
  cols!: TableColumn[];
  constructor(private orderService:OrderService,
    private customerService:CustomerService
    ) {}

  ngOnInit(): void {
    this.cols = [
      { field: 'id', header: 'Orders Summary Id', sortable: true, filtrable: true, type: ColumnType.Link },
      { field: 'ordersSummaryDesc', header: 'Orders Description', sortable: true, filtrable: true},
      { field: 'totalAmount', header: 'Total Amount', sortable: false, filtrable: true },
      { field: 'isCash', header: 'Cash', sortable: false, filtrable: true },
      { field: 'isActive', header: 'Active', sortable: false, filtrable: true },
      { field: 'createdOn', header: 'Orders On', sortable: true, filtrable: false,type:ColumnType.DateTime,dateFormat:Format.DATE },
      { field: 'customerId', header: 'Customer Id', sortable: false, filtrable: true, type:ColumnType.SubLink,route:'../customers/' }
    ] as TableColumn[];
    this.orderService.getOrdersSummaries().subscribe((ordersSummaries)=>{
      this.ordersSummaries = ordersSummaries;
    });
  }
  onRowEditSave(event: ModifiedTableRow) {
    console.log(event)
  }
  onEnterPress($event:any){
    let barcode = this.barcode;
    this.customerService.getCustomerByBarcode(barcode).subscribe((customer)=>{
      this.orderService.getOrdersSummariesByCustomerId(customer.id).subscribe((ordersSummaries)=>{
        this.ordersSummaries = ordersSummaries;
      });
    });
  }
}
