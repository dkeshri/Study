export interface Country {
    id:number;
    name:string;
    iso3:string;
    iso2:string;
    capital:string;
    currency:string;
    currencySymbol:string;
    region:string;
}
export interface State {
    id:number;
    name:string;
    countryCode:string;
    stateCode:string;
    Type:string;
    latitude:number;
    longitude:number;
}
export interface City {
    id:number;
    name:string;
    countryCode:string;
    stateCode:string;
    latitude:number;
    longitude:number;
}