import { Component, OnInit } from '@angular/core';
import { ColumnType, TableColumn } from 'src/app/theme/models/table-model';
import { Order } from '../Model/order-model';
import { OrderService } from 'src/app/services/order.service';
import { ActivatedRoute } from '@angular/router';
import { OrderPdfData, PdfColumnType, PdfTable, PdfTableColumn, PdfTableColumnAlign } from 'src/app/core/model/pdf.model';
import { PdfService } from 'src/app/core/services/pdf.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.css']
})
export class OrderDetailsComponent implements OnInit {
  orders!: Order[];
  isEditable = false;
  totalAmount : number = 0;
  cols: TableColumn[] = [
    { field: 'id', header: 'Order Code', sortable: true, filtrable: true, type: ColumnType.Link },
    { field: 'procuctName', header: 'Product Name', sortable: true, filtrable: true},
    { field: 'price', header: 'Price', sortable: false, filtrable: true },
    { field: 'quantity', header: 'Quantity', sortable: false, filtrable: true },
    { field: 'amount', header: 'Amount', sortable: false, filtrable: true }
  ] as TableColumn[];
  orderSummaryId!:string;
  constructor(
    private orderSevice : OrderService,
    private route: ActivatedRoute,
    private pdfService:PdfService
    ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.orderSummaryId = params['orderId'];
      this.orderSevice.getOrdersByOrderSummaryId(+this.orderSummaryId).subscribe((orders)=>{
        this.orders = orders;
        orders.forEach((item)=>{
          this.totalAmount = item.amount + this.totalAmount;
        })
      });
    });
  }
  onRowEditSave(event:any){

  }
  downloadPdf(){
    let pdfData = {} as OrderPdfData;
    pdfData.customerId = 1;
    pdfData.totalAmount = this.totalAmount;
    pdfData.ordersSummaryId = +this.orderSummaryId;
    
    let ordersTable = {} as PdfTable;
    ordersTable.columns = [
      {field:'id',header:'Order Code'},
      {field:'procuctName',header:'Product Name'},
      {field:'price',header:'Price'},
      {field:'quantity',header:'Quantity'},
      {field:'amount',header:'Amount',alignment:PdfTableColumnAlign.RIGHT}
    ] as PdfTableColumn[];
    ordersTable.data = this.orders;
    pdfData.ordersTable = ordersTable;
    this.pdfService.createOrdersPdf(pdfData);
  }
}
