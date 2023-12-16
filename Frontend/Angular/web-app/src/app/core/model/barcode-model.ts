export interface BarcodeCreate{
    code:string,
    width:number,
    height:number,
    includeLabel:boolean
}
export interface Barcode{
    image:string;
}