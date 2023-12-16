import { Option } from "src/app/theme/models/table-model";

export interface Address extends CreateAddress{
    id:number;
    customerId:number;
}
export interface CreateAddress{
    address1:string;
    address2:string;
    landmark:string;
    city:string;
    pincode:number;
    state:string;
    country:string;
    isPermanent:boolean;
}

export interface GeoRegionOptions{
    city:Option[];
    state:Option[];
    country:Option[];
}