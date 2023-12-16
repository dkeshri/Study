export interface CreateUnit{
    name:string;
    shortName:string;
    isActive:boolean;
    userId:number;
   
}
export interface Unit extends CreateUnit{
    id:number;
    user:string;   
}
