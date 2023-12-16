import { Component, OnInit } from '@angular/core';
import { InvoiceService } from 'src/app/services/invoice.service';
import { Invoice } from 'src/app/services/models/invoice.model';
import { Format } from 'src/app/shared/models/app-constants';
import { ColumnType, TableColumn } from 'src/app/theme/models/table-model';
import { CustomerService } from '../../customers/services/customer.service';

@Component({
  selector: 'app-invoices',
  templateUrl: './invoices.component.html',
  styleUrls: ['./invoices.component.css']
})
export class InvoicesComponent implements OnInit {
  barcode:string = '';
  customerId: string = '';
  invoiceCols: TableColumn[] = [
    { field: 'id', header: 'Invoice Id', sortable: true, filtrable: true, type: ColumnType.Link },
    { field: 'from', header: 'From', sortable: true, filtrable: true, type:ColumnType.DateTime, dateFormat:Format.DATE},
    { field: 'to', header: 'To', sortable: false, filtrable: false, type:ColumnType.DateTime, dateFormat:Format.DATE },
    { field: 'totalAmount', header: 'Total Amount', sortable: false, filtrable: false },
    { field: 'amountReceived', header: 'Amount Received', sortable: false, filtrable: false },
    { field: 'amountReceivedOn', header: 'Received On', sortable: false, filtrable: false, type:ColumnType.DateTime },
    { field: 'arrears', header: 'Arrears', sortable: false, filtrable: false }

  ] as TableColumn[];
  invoices!: Invoice[];
  isInvoiceEditable : boolean = false;
  constructor(
    private invoiceService:InvoiceService,
    private customerService:CustomerService
    ) { }

  ngOnInit(): void {
  }
  onEnterPress($event:any){
    let barcode = this.barcode;
    this.invoices = [] as Invoice[]
    this.customerService.getCustomerByBarcode(barcode).subscribe((customer)=>{
     this.customerId = customer.id.toString();
     this.invoiceService.getInvoicesByCustomerId(customer.id).subscribe((invoices)=>{
      this.invoices = invoices;
    });
    });
  }
}
