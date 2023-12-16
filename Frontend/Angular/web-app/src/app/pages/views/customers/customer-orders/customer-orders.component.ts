import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Arrear, Customer } from '../models/customer.model';
import { CustomerService } from '../services/customer.service';
import { ColumnType, TableColumn } from 'src/app/theme/models/table-model';
import { OrderService } from 'src/app/services/order.service';
import { OrdersSummary } from '../../orders/Model/order-model';
import { InvoiceService } from 'src/app/services/invoice.service';
import { Invoice } from 'src/app/services/models/invoice.model';
import { PaymentService } from 'src/app/services/payment.service';
import { Payment } from 'src/app/services/models/payment.model';
import { Format, RouteAction, TableProperty } from 'src/app/shared/models/app-constants';
import { MatLegacyTabChangeEvent as MatTabChangeEvent } from '@angular/material/legacy-tabs';
@Component({
  selector: 'app-customer-orders',
  templateUrl: './customer-orders.component.html',
  styleUrls: ['./customer-orders.component.css']
})
export class CustomerOrdersComponent implements OnInit {
  orderSummaryCols: TableColumn[] = [
    { field: 'id', header: 'Orders', sortable: true, filtrable: true, type: ColumnType.SubLink, route: '../../../orders/' },
    { field: 'ordersSummaryDesc', header: 'Orders Description', sortable: false, filtrable: false },
    { field: 'totalAmount', header: 'Total Amount', sortable: false, filtrable: false },
    { field: 'createdOn', header: 'Orders On', sortable: true, filtrable: false, type: ColumnType.DateTime, dateFormat: Format.DATE }
  ] as TableColumn[];
  invoiceCols: TableColumn[] = [
    { field: 'id', header: 'Invoice Id', sortable: true, filtrable: true, type: ColumnType.SubLink, route: '../../../invoices/' },
    { field: 'from', header: 'From', sortable: true, filtrable: true, type: ColumnType.DateTime, dateFormat: Format.DATE },
    { field: 'to', header: 'To', sortable: false, filtrable: false, type: ColumnType.DateTime, dateFormat: Format.DATE },
    { field: 'totalAmount', header: 'Total Amount', sortable: false, filtrable: false },
    { field: 'amountReceived', header: 'Amount Received', sortable: false, filtrable: false },
    { field: 'amountReceivedOn', header: 'Received On', sortable: false, filtrable: false, type: ColumnType.DateTime },
    { field: 'arrears', header: 'Arrears', sortable: false, filtrable: false }

  ] as TableColumn[];

  paymentCols: TableColumn[] = [
    { field: 'id', header: 'Payment Id', sortable: true, filtrable: true, type: ColumnType.Link },
    { field: 'amount', header: 'Amount', sortable: false, filtrable: false },
    { field: 'createdOn', header: 'Paid On', sortable: true, filtrable: false, type: ColumnType.DateTime },
    { field: 'invoiceNumber', header: 'Invoice Id', sortable: false, filtrable: false }
  ] as TableColumn[];

  isOrderSummaryEditable: boolean = false;
  isInvoiceEditable: boolean = false;
  isPaymentEditable: boolean = false;
  totalUnpaidAmount: number = 0;
  customerId?: any;
  customer = {} as Customer;
  ordersSummaries!: OrdersSummary[];
  invoices!: Invoice[];
  payments!: Payment[];
  addInvoiceRouteLink!: string;
  arrear!: Arrear;
  lastPaymentAmount: number = 0;
  lastPaymentDate: Date = new Date();
  invoiceColIndex: number = 1;
  invoiceSortOrder: number = TableProperty.SORT_DESC;
  paymentSortColIndex: number = 2;
  paymentSortOrder: number = TableProperty.SORT_DESC;
  orderMatLabel = "ORDERS";
  invoicesMatLabel = "INVOICES";
  paymentsMatLabel = "PAYMENTS";
  constructor(
    private route: ActivatedRoute,
    private customerService: CustomerService,
    private orderService: OrderService,
    private invoiceService: InvoiceService,
    private paymentService: PaymentService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      if (params.has('customerId')) {
        this.customerId = params.get('customerId') ? params.get('customerId') : '';
        this.addInvoiceRouteLink = `../../../invoices/${RouteAction.ADD}/${this.customerId}`;
        this.customerService.getCustomerById(this.customerId).subscribe((customer) => {
          this.customer = customer;
        });
        this.getArrearOfCustomer(this.customerId);
        this.paymentService.getPaymentsByCustomerId(this.customerId).subscribe((payments: Payment[]) => {
          this.payments = payments;
          if (payments.length > 0) {
            this.lastPaymentAmount = payments[0].amount;
            this.lastPaymentDate = new Date(payments[0].createdOn);
          }
        });
      }
    });

  }
  private getArrearOfCustomer(customerId: string) {
    this.customerService.getCustomerArrearByCustomerId(customerId).subscribe((arrear: Arrear) => {
      this.arrear = arrear;
      this.intiFormData(customerId, new Date());
    });
  }

  private intiFormData(customerId: string, to: Date) {
    let from = new Date(this.arrear.lastInvoiceToDate);
    let prevArrear = this.arrear.arrears;
    this.orderService.getOrdersSummariesByFromToDate(+customerId, from, to).subscribe((ordersSummaries) => {
      this.ordersSummaries = ordersSummaries;
      this.setFormOnOrderSummaries(ordersSummaries, prevArrear);
    });
  }

  private setFormOnOrderSummaries(ordersSummaries: OrdersSummary[], prevArrear: number) {
    let billAmount = this.calculateBillAmountFromOrderSummaries(ordersSummaries);
    let totalAmount = billAmount + prevArrear;
    this.totalUnpaidAmount = totalAmount;
  }
  private calculateBillAmountFromOrderSummaries(ordersSummaries: OrdersSummary[]): number {
    let total = 0;
    ordersSummaries.forEach((item) => {
      total = total + item.totalAmount;
    });
    return total;
  }
  onTabClick(event: MatTabChangeEvent) {
    let tab = event.tab.textLabel;
    switch (tab) {
      case this.paymentsMatLabel:
        if (!this.payments) {
          this.paymentService.getPaymentsByCustomerId(this.customerId).subscribe((payments: Payment[]) => {
            this.payments = payments;
            if (payments.length > 0) {
              this.lastPaymentAmount = payments[0].amount;
              this.lastPaymentDate = new Date(payments[0].createdOn);
            }
          });
        }
        break;
      case this.invoicesMatLabel:
        if (!this.invoices) {
          this.invoiceService.getInvoicesByCustomerId(this.customerId).subscribe((invoices) => {
            this.invoices = invoices;
          });
        }
        break;
      default:
        console.log("default");
    }
  }
}
