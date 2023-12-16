import { GovDocument } from "src/app/pages/views/customers/models/customer.model";
import { Address } from "./address.model";
import { Shop } from "./shop.model";

export interface User {
    id:number;
    addresses:Address[];
    name:string;
    phone:string;
    username:string;
    password:string;
    role:string;
    isActive:boolean;
    shops:Shop[];
    documents:GovDocument[];

}
export interface UserRole{
    id:number;
    role:string;
    name:string;
}