import { ColumnType, TableColumn, ModifiedTableRow, Option } from './../../../../theme/models/table-model';
import { Component, OnInit } from '@angular/core';
import { faCoffee } from '@fortawesome/free-solid-svg-icons';
import { Table } from 'primeng/table';
import { Customer } from '../models/customer.model';
import { CustomerService } from '../services/customer.service';
@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})
export class CustomersComponent implements OnInit {
  public customer:string = '';
  barcode:string = '';
  customers: Customer[];
  cols: TableColumn[];
  isEditable:boolean = false;
    constructor(private customerService:CustomerService) {
     this.cols = [];
     this.customers = []
    }

  ngOnInit(): void {

    this.cols = [
      { field: 'id', header: 'Customer Code', sortable:true,filtrable:true ,type:ColumnType.Link},
      { field: 'barcode', header: 'Customer Barcode', sortable:true, filtrable:true},
      { field: 'name', header: 'Customer Name', sortable:true,filtrable:true},
      { field: 'isActive', header: 'isActive', sortable:false ,filtrable:false},
      { field: 'id', header: 'Orders', sortable: false, filtrable: false, type:ColumnType.DetailNavBtn,route:'/orders',btnValue:'Orders Detail' }
  ] as TableColumn[];
  this.customerService.getCustomers().subscribe((customers)=>{
    this.customers = customers;
  });
  }
  onEnterPress($event:any){
    let barcode = this.barcode;
    console.log(barcode);
  }
  clear(table: Table) {
    table.clear();
  }

  onRowEditSave(event: ModifiedTableRow) {
    console.log(event)
  }
}
