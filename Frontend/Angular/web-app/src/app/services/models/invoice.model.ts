export interface Invoice {
    id:number;
    from:Date;
    to:Date;
    billAmount:number;
    prevArrears:number;
    discount:number;
    totalAmount:number
    amountReceived:number;
    amountReceivedOn:Date;
    arrears:number;
    customerId:number;
}