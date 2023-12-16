export interface Product {
    id: number;
    name: string;
    price: number;
    unitId: number;
    secUnitId: number;
    costPrice: number;
    barcode: string;
    expireOn: Date;
    isActive:boolean;
    unitConversionId:number;
}
export interface CreateProduct {
    name: string;
    price: string;
    unitId: string;
    secUnitId: number;
    costPrice: number;
    barcode: string;
    expireOn: Date;
    unitConversionId:number;
    isActive:boolean;
}