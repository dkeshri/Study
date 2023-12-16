import { EntityBase } from "src/app/core/model/entity-base.model";
import { Option } from "src/app/theme/models/table-model";

export interface Order {
    id:number;
    barcode: string;
    productId:number;
    procuctName: string;
    unitId: number;
    secUnitId:number;
    unitConversionRate:number;
    price: number;
    quantity: number;
    amount: number;
    units:Option[];
  }

export interface OrdersSummary extends EntityBase{
    id:number;
    ordersSummaryDesc:string;
    totalAmount:number;
    isCash:boolean;
    isActive:boolean;
    customerId:number;
    orders:Order[];
}