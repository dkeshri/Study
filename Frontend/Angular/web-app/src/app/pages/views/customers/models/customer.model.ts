import { Address, CreateAddress } from "src/app/services/models/address.model";

export interface GovDocument extends CreateGovDocument{
    id:number;
}
export interface CreateGovDocument{
    docId:string;
    type:string;
    isVerified:boolean;
}
export interface Customer{
    id:number;
    barcode:string;
    qrcode:string;
    name:string;
    phone:number;
    shopId:number;
    isActive:boolean;
    addresses:Address[];
    documents:GovDocument[];
}
export interface CreateCustomer{
    name:string;
    phone:number;
    shopId:number;
    isActive:boolean;
    addresses:Address[];
    documents:CreateGovDocument[];
}

export interface Arrear{
    id:number;
    arrears:number;
    invoiceNumber:number;
    lastInvoiceToDate:Date;
    customerId:number;
}