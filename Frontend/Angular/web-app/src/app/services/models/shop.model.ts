import { Address } from "./address.model";


export interface Shop{
    id:number;
    name:string;
    addresses:Address[];
    phone:number;
    isActive:boolean;
    userId:number;
}