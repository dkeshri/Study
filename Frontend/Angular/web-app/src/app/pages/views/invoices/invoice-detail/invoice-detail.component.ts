import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { OrdersSummary } from '../../orders/Model/order-model';
import { ColumnType, ModifiedTableRow, TableColumn } from 'src/app/theme/models/table-model';
import { CustomerService } from '../../customers/services/customer.service';
import { OrderService } from 'src/app/services/order.service';
import { ActivatedRoute } from '@angular/router';
import { Action, RouteAction } from 'src/app/shared/models/app-constants';
import { Invoice } from 'src/app/services/models/invoice.model';
import { InvoiceService } from 'src/app/services/invoice.service';
import { MessageService } from 'primeng/api';
import { Arrear } from '../../customers/models/customer.model';
import { PdfService } from 'src/app/core/services/pdf.service';
import { InvoiceOrderSummaryPdfData, PdfColumnType, PdfTable, PdfTableColumn, PdfTableColumnAlign } from 'src/app/core/model/pdf.model';

@Component({
  selector: 'app-invoice-detail',
  templateUrl: './invoice-detail.component.html',
  styleUrls: ['./invoice-detail.component.css']
})
export class InvoiceDetailComponent implements OnInit {

  invoiceFormGroup: FormGroup = this.formBuilder.group({
    from:[null],
    to: [new Date()],
    billAmount: [0],
    prevArrears:[0],
    discount:[0],
    totalAmount:[0],
    amountReceived: [0],
    arrears:[0]
  });
  ordersSummaries!: OrdersSummary[];
  customerId:any;
  invoiceId:any;
  invoiceOldValue!:Invoice;
  arrear!:Arrear;
  cols: TableColumn[] = [
    { field: 'id', header: 'Orders Summary Id', sortable: true, filtrable: true, type: ColumnType.SubLink,route:'../../orders/' },
      { field: 'ordersSummaryDesc', header: 'Description', sortable: true, filtrable: true},
      { field: 'totalAmount', header: 'Total Amount', sortable: false, filtrable: true },
      { field: 'isCash', header: 'Cash', sortable: false, filtrable: true },
      { field: 'isActive', header: 'Active', sortable: false, filtrable: true },
      { field: 'customerId', header: 'Customer Id', sortable: false, filtrable: false }
  ] as TableColumn[];

  minimumDate !:Date;
  maxDate:Date = new Date();
  constructor(
    private formBuilder: FormBuilder,
    private orderService:OrderService,
    private route: ActivatedRoute,
    private invoiceService:InvoiceService,
    private messageService: MessageService,
    private customerService: CustomerService,
    private pdfService:PdfService
    ) { }
  

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      if(params.has('invoiceId')){
        this.invoiceId = params.get('invoiceId') ?? '';
        if(this.invoiceId === RouteAction.ADD){
          if(params.has('customerId')){
            this.customerId = params.get('customerId') ?? '';
            this.getArrearOfCustomer(this.customerId);
          }
        }else{
          this.invoiceService.getInvoice(this.invoiceId).subscribe((invoice)=>{
            this.invoiceOldValue = invoice;
            this.setFormOnGetInvoice(invoice);
          });    
        }
      }
    });
    
  }

  private getArrearOfCustomer(customerId:string){
    this.customerService.getCustomerArrearByCustomerId(customerId).subscribe((arrear:Arrear)=>{
      this.arrear = arrear;
      this.invoiceFormGroup.controls['from'].setValue(new Date(arrear.lastInvoiceToDate));
      let to = this.invoiceFormGroup.controls['to'].value as Date;
      this.intiFormData(customerId,to); 
    });
  }

  private intiFormData(customerId:string,to:Date){
    let from = new Date(this.arrear.lastInvoiceToDate);
    this.minimumDate = from;
    let prevArrear = this.arrear.arrears;
    this.orderService.getOrdersSummariesByFromToDate(+customerId,from,to).subscribe((ordersSummaries)=>{
      this.ordersSummaries = ordersSummaries;
      this.setFormOnOrderSummaries(ordersSummaries,prevArrear);
    });
  }

  private setFormOnOrderSummaries(ordersSummaries:OrdersSummary[], prevArrear:number){
    let billAmount = this.calculateBillAmountFromOrderSummaries(ordersSummaries);
    let totalAmount = billAmount + prevArrear;
    this.invoiceFormGroup.controls['billAmount'].setValue(billAmount);
    this.invoiceFormGroup.controls['prevArrears'].setValue(prevArrear);
    this.invoiceFormGroup.controls['totalAmount'].setValue(totalAmount);
  }

  private calculateBillAmountFromOrderSummaries(ordersSummaries : OrdersSummary[]) : number{
    let total = 0;
    ordersSummaries.forEach((item)=>{
      total = total + item.totalAmount;
    });
    return total;
  }
  private setFormOnGetInvoice(invoice: Invoice){
    this.invoiceFormGroup.controls['from'].setValue(new Date(invoice.from));
    this.invoiceFormGroup.controls['to'].setValue(new Date(invoice.to));
    this.invoiceFormGroup.controls['billAmount'].setValue(invoice.billAmount);
    this.invoiceFormGroup.controls['prevArrears'].setValue(invoice.prevArrears);
    this.invoiceFormGroup.controls['discount'].setValue(invoice.discount);
    this.invoiceFormGroup.controls['totalAmount'].setValue(invoice.totalAmount);
    this.invoiceFormGroup.controls['amountReceived'].setValue(invoice.amountReceived);
    this.invoiceFormGroup.controls['arrears'].setValue(invoice.arrears);
    if(!this.customerId){
      this.invoiceFormGroup.controls['amountReceived'].disable();
      this.invoiceFormGroup.controls['to'].disable();
      this.invoiceFormGroup.controls['discount'].disable();
    }
    this.orderService.getOrdersSummariesByFromToDate(invoice.customerId,new Date(invoice.from),new Date(invoice.to)).subscribe((ordersSummaries)=>{
      this.ordersSummaries = ordersSummaries;
    });
  }

  formData(action?: Action): Invoice {
    let invoice = {} as Invoice;
    if (action == Action.UPDATE) {
      invoice.id = this.invoiceOldValue.id;
    }
    invoice.from = this.invoiceFormGroup.controls['from'].value;
    invoice.to = this.invoiceFormGroup.controls['to'].value;
    invoice.billAmount = this.invoiceFormGroup.controls['billAmount'].value;
    invoice.prevArrears = this.invoiceFormGroup.controls['prevArrears'].value;
    invoice.discount = this.invoiceFormGroup.controls['discount'].value;
    invoice.totalAmount = this.invoiceFormGroup.controls['totalAmount'].value;
    invoice.amountReceived = +this.invoiceFormGroup.controls['amountReceived'].value;
    invoice.arrears = invoice.totalAmount - invoice.amountReceived;
    invoice.customerId = this.customerId;
    invoice.amountReceivedOn = new Date();
    return invoice;
  }
  onSave(){
    let invoice = this.formData();
    this.invoiceService.createInvoice(invoice).subscribe((data) => {
      this.messageService.add({ severity: 'success', summary: 'Invoice Created Successfully', detail: data.id.toString(), life: 3000 });
    });
  }
  onRowEditSave(event: ModifiedTableRow) {
    console.log(event)
  }
  onOrderDateChange(){
    if(this.customerId){
      let to = new Date(this.invoiceFormGroup.controls['to'].value);
      this.intiFormData(this.customerId,to);
    }
  }
  onDiscountChange(){
    if(this.customerId){
      let billAmount = this.invoiceFormGroup.controls['billAmount'].value;
      let prevArrear = this.invoiceFormGroup.controls['prevArrears'].value;
      let discount = this.invoiceFormGroup.controls['discount'].value;
      let totalAmount = billAmount + prevArrear - discount;
      this.invoiceFormGroup.controls['totalAmount'].setValue(totalAmount);
    }
  }

  downloadPdf(){
    let pdfData = {} as InvoiceOrderSummaryPdfData;
    pdfData.invoiceId = this.invoiceOldValue.id;
    pdfData.customerId = this.invoiceOldValue.customerId;
    pdfData.invoiceFrom = this.invoiceOldValue.from;
    pdfData.invoiceTo = this.invoiceOldValue.to;
    pdfData.prevArrear = this.invoiceOldValue.prevArrears;
    pdfData.arrear = this.invoiceOldValue.arrears;
    pdfData.billAmount = this.invoiceOldValue.billAmount;
    pdfData.discount = this.invoiceOldValue.discount;
    pdfData.totalAmount = this.invoiceOldValue.totalAmount;
    pdfData.paidAmount = this.invoiceOldValue.amountReceived;
    pdfData.paymentDate = new Date();
    
    let orderSummaryTable = {} as PdfTable;
    orderSummaryTable.columns = [
      {field:'id',header:'Order Id'},
      {field:'ordersSummaryDesc',header:'Orders Description'},
      {field:'createdOn',header:'Order On',type:PdfColumnType.DateTime},
      {field:'totalAmount',header:'Amount',alignment:PdfTableColumnAlign.RIGHT}
    ] as PdfTableColumn[];
    orderSummaryTable.data = this.ordersSummaries;
    pdfData.orderSummaryTable = orderSummaryTable;
    this.pdfService.createInvoiceOrderSummaryPdf(pdfData);
  }
}
