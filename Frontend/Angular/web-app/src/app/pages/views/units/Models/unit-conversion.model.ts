export interface UnitConversion {
    id:number;
    primaryId: number;
    secId: number;
    value: number;
    label:string;
}
export interface UnitConversionParam extends UnitConversion{
    primaryUnitText:string;
    secUnitText:string;
}