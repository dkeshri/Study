import { Injectable } from '@angular/core';
import * as pdfMake from "pdfmake/build/pdfmake";
import * as pdfFonts from 'pdfmake/build/vfs_fonts';
import { InvoiceOrderSummaryPdfData, OrderPdfData, PdfColumnType, PdfTable } from '../model/pdf.model';
import { CustomerService } from 'src/app/pages/views/customers/services/customer.service';
import { PaymentService } from 'src/app/services/payment.service';
import { ShopService } from 'src/app/services/shop.service';
import { AuthenticationService } from './auth/authentication.service';
import { forkJoin } from 'rxjs';
import { DatetimePipe } from 'src/app/shared/datetime.pipe';
import { Format } from 'src/app/shared/models/app-constants';
import { DatePipe } from '@angular/common';
import { OrderService } from 'src/app/services/order.service';

(<any>pdfMake).vfs = pdfFonts.pdfMake.vfs;
@Injectable({
  providedIn: 'root'
})
export class PdfService {

  constructor(
    private customerService:CustomerService,
    private paymentService:PaymentService,
    private shopService:ShopService,
    private authenticationService:AuthenticationService,
    private dateTimePipe:DatetimePipe,
    private orderService:OrderService
    ) { }

  private createPdf(docDefinition: any,fileName:string) {
    pdfMake.createPdf(docDefinition).download(fileName);
  }
  public createInvoiceOrderSummaryPdf(invoiceData:InvoiceOrderSummaryPdfData){
    let shopId = this.authenticationService.getUserSelectedShopId();
    forkJoin(
      [
        this.shopService.getShop(shopId),
        this.customerService.getCustomerById(invoiceData.customerId.toString()),
        this.paymentService.getPaymentDetailByInvoiceId(invoiceData.invoiceId.toString())
      ]
    ).subscribe(([shop, customer, paymentDetail]) => {
      invoiceData.shop = shop;
      invoiceData.customer = customer;
      invoiceData.paymentDate = paymentDetail.createdOn;
      let fileName = 'Invoice_'+this.dateTimePipe.transform(invoiceData.invoiceFrom,Format.DATE)
      +'_'+this.dateTimePipe.transform(invoiceData.invoiceTo,Format.DATE)
      this.createPdf(this.getDocDefForInvoiceOrderSummaryPdf(invoiceData),fileName);
    });
  }
  private buildTableBody(table:PdfTable) {
    let body = [] as any[];
    let columns = [] as any[];

    table.columns.forEach((column)=>{
      columns.push({text:column.header,alignment:column.alignment,bold:true});
    });
    body.push(columns);

    table.data.forEach(function(row) {
        let dataRow = [] as any[];
        table.columns.forEach(function(column) {
            let value = row[column.field]??'';
            switch(column.type){
              case PdfColumnType.DateTime:
                value = new Date(value);
                value = new DatetimePipe().transform(value, Format.DATE);
                break;
            }
            dataRow.push({ text: value,alignment:column.alignment});
        })
        body.push(dataRow);
    });

    return body;
}
  private getDocDefForInvoiceOrderSummaryPdf(data:InvoiceOrderSummaryPdfData) {
    let paymentDate = this.dateTimePipe.transform(data.paymentDate);
    let invoiceFrom = this.dateTimePipe.transform(data.invoiceFrom,Format.DATE);
    let invoiceTo = this.dateTimePipe.transform(data.invoiceTo,Format.DATE);
    let invoicCreateDate = this.dateTimePipe.transform(new Date(),Format.DATE);

    let customerAddress = data.customer.addresses.find(item => item.isPermanent === false);
    let shopAddress = data.shop.addresses.find(item => item.isPermanent === false);
    let body = this.buildTableBody(data.orderSummaryTable);

    let docDefinition = {
      pageSize: 'A4',
      // [left, top, right, bottom] or [horizontal, vertical] or just a number for equal margins
      //pageMargins: [ 40, 60, 40, 60 ],
      header: {
        text: 'Invoice',
        style: 'header'
      },
      content: [
        {
          canvas: [
            {
              type: 'line',
              x1: 0, y1: 0,
              x2: 515, y2: 0,
              lineWidth: 1
            }
          ]
        },
        {
          // use when want to apply multiple css
          style: ['contentSpace'],
          columns: [
            {
              stack: [
                {
                  bold: true,
                  text: data.shop.name
                },
                data.shop.phone,
                shopAddress?.address1,
                shopAddress?.city
              ]
            },
            {
              stack: [
                {
                  bold: true,
                  text: data.customer.name
                },
                data.customer.phone,
                customerAddress?.address1,
                customerAddress?.city
              ]
            },
            {
              columns: [
                {
                  alignment: 'right',
                  stack: [
                    {
                      bold: true,
                      text: 'Date'
                    },
                    'Bill Amount',
                    {
                      style: 'arrear',
                      text: 'Prev. Arrear'
                    },
                    {
                      style: 'discountTitle',
                      text: 'Discount'
                    },
                    {
                      style: ['totalTitle', 'contentSpace'],
                      text: 'Total'
                    },
                    {
                      color: 'green',
                      style: 'totalTitle',
                      text: 'Paid'
                    },
                    {
                      style: ['totalTitle','arrear'],
                      text: 'Arrear'
                    }
                  ]
                },
                {
                  alignment: 'right',
                  stack: [
                    invoicCreateDate,
                    data.billAmount,
                    {
                      style: 'arrear',
                      text: data.prevArrear
                    },
                    {
                      style: 'discountTitle',
                      text: data.discount
                    },
                    {
                      style: ['totalTitle', 'contentSpace'],
                      text: data.totalAmount
                    },
                    {
                      color: 'green',
                      style: 'totalTitle',
                      text: data.paidAmount
                    },
                    {
                      style: ['totalTitle','arrear'],
                      text: data.arrear
                    }
                  ]
                }
              ]
            }
          ]
        },
        {
          style: 'contentSpace',
          text: [
            {
              style: 'invoicePeriodTitle',
              text: 'Payment On : '
            },
            paymentDate
          ]
        },
        {
          text: [
            {
              style: 'invoicePeriodTitle',
              text: 'Invoice Period : '
            },
            invoiceFrom + ' - ' + invoiceTo
          ]
        },
        {
          layout: 'lightHorizontalLines', // optional
          style: 'orderSummaryTable',
          table: {
            // headers are automatically repeated if the table spans over multiple pages
            // you can declare how many rows should be treated as headers
            headerRows: 1,
            widths: [100, '*', 100, 100],
            body: body
          }
        }
      ],
      styles: {
        header: {
          fontSize: 22,
          bold: true,
          alignment: 'center'
        },
        invoicePeriodTitle: {
          fontSize: 13,
          bold: true
        },
        orderSummaryTable: {
          margin: [0, 15]
        },
        tableOpacityExample: {
          margin: [0, 5, 0, 15],
          fillColor: 'blue',
          fillOpacity: 0.3
        },
        tableHeader: {
          bold: true,
          fontSize: 13,
          color: 'black'
        },
        contentSpace: {
          margin: [0, 10, 0, 0]
        },
        arrear: {
          color: 'red'
        },
        totalTitle: {
          bold: true,
          fontSize: 13,
          color: 'black'
        },
        discountTitle: {
          color: 'green'
        }
      },
      defaultStyle: {
        alignment: 'justify'
      }
    };
    return docDefinition;
  }

  public createOrdersPdf(invoiceData:OrderPdfData){
    let shopId = this.authenticationService.getUserSelectedShopId();
    forkJoin(
      [
        this.shopService.getShop(shopId),
        this.customerService.getCustomerById(invoiceData.customerId.toString()),
        this.orderService.getOrdersSummaryById(invoiceData.ordersSummaryId.toString())
      ]
    ).subscribe(([shop, customer,ordersSummary]) => {
      invoiceData.shop = shop;
      invoiceData.customer = customer;
      invoiceData.ordersSummary = ordersSummary;
      let fileName = 'Orders_'+this.dateTimePipe.transform(invoiceData.ordersSummary.createdOn,Format.DATE);
      this.createPdf(this.getDocDefForOrdersPdf(invoiceData),fileName);
    });
  }
  private getDocDefForOrdersPdf(data:OrderPdfData) {
    let orderCreateDate = this.dateTimePipe.transform(data.ordersSummary.createdOn,Format.DATE);
    let totalAmount = data.totalAmount;
    let orderSummaryId = data.ordersSummaryId;
    let customerAddress = data.customer.addresses.find(item => item.isPermanent === false);
    let shopAddress = data.shop.addresses.find(item => item.isPermanent === false);
    let body = this.buildTableBody(data.ordersTable);

    let docDefinition = {
      pageSize: 'A4',
      // [left, top, right, bottom] or [horizontal, vertical] or just a number for equal margins
      //pageMargins: [ 40, 60, 40, 60 ],
      header: {
        text: 'Orders',
        style: 'header'
      },
      content: [
        {
          canvas: [
            {
              type: 'line',
              x1: 0, y1: 0,
              x2: 515, y2: 0,
              lineWidth: 1
            }
          ]
        },
        {
          // use when want to apply multiple css
          style: ['contentSpace'],
          columns: [
            {
              stack: [
                {
                  bold: true,
                  text: data.shop.name
                },
                data.shop.phone,
                shopAddress?.address1,
                shopAddress?.city
              ]
            },
            {
              stack: [
                {
                  bold: true,
                  text: data.customer.name
                },
                data.customer.phone,
                customerAddress?.address1,
                customerAddress?.city
              ]
            },
            {
              columns: [
                {
                  alignment: 'right',
                  stack: [
                    {
                      bold: true,
                      text: 'Date'
                    },
                    'SummaryId',
                    {
                      style: ['totalTitle', 'contentSpace'],
                      text: 'Total Amount'
                    }
                  ]
                },
                {
                  alignment: 'right',
                  stack: [
                    orderCreateDate,
                    orderSummaryId,
                    {
                      style: ['totalTitle', 'contentSpace'],
                      text: totalAmount
                    }
                  ]
                }
              ]
            }
          ]
        },
        {
          layout: 'lightHorizontalLines', // optional
          style: 'orderSummaryTable',
          table: {
            // headers are automatically repeated if the table spans over multiple pages
            // you can declare how many rows should be treated as headers
            headerRows: 1,
            widths: [70, '*', 50, 50,50],
            body: body
          }
        }
      ],
      styles: {
        header: {
          fontSize: 22,
          bold: true,
          alignment: 'center'
        },
        invoicePeriodTitle: {
          fontSize: 13,
          bold: true
        },
        orderSummaryTable: {
          margin: [0, 15]
        },
        tableOpacityExample: {
          margin: [0, 5, 0, 15],
          fillColor: 'blue',
          fillOpacity: 0.3
        },
        tableHeader: {
          bold: true,
          fontSize: 13,
          color: 'black'
        },
        contentSpace: {
          margin: [0, 10, 0, 0]
        },
        arrear: {
          color: 'red'
        },
        totalTitle: {
          bold: true,
          fontSize: 12,
          color: 'black'
        },
        discountTitle: {
          color: 'green'
        }
      },
      defaultStyle: {
        alignment: 'justify'
      }
    };
    return docDefinition;
  }
}
