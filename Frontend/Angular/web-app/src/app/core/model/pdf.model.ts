import { Customer } from "src/app/pages/views/customers/models/customer.model";
import { OrdersSummary } from "src/app/pages/views/orders/Model/order-model";
import { Address } from "src/app/services/models/address.model";
import { Shop } from "src/app/services/models/shop.model";

export interface InvoiceOrderSummaryPdfData{
    invoiceId:number;
    customerId:number;
    invoiceFrom:Date;
    invoiceTo:Date;
    billAmount:number;
    prevArrear:number;
    arrear:number;
    totalAmount:number;
    discount:number;
    paidAmount:number;
    paymentDate:Date;
    shop:Shop;
    customer:Customer;
    orderSummaryTable:PdfTable;
}
export interface PdfTableColumn{
    header:string, // string that will be display in column header,
    field:string,
    type?:PdfColumnType,
    alignment?:string
}
export interface PdfTable{
    columns:PdfTableColumn[];
    data:any[];
}
export enum PdfColumnType{
    DateTime,
    Link
}
export class PdfTableColumnAlign{
    static readonly LEFT = 'left';
    static readonly RIGHT = 'right';
    static readonly CENTER = 'center';
    static readonly JUSTIFY = 'justify';
}
export interface OrderPdfData{
    customerId:number;
    totalAmount:number;
    ordersSummaryId:number;
    shop:Shop;
    customer:Customer;
    ordersTable:PdfTable;
    ordersSummary:OrdersSummary;
}