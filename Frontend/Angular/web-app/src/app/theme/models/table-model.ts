
export interface Option{
    value:string | number;
    label:string;
    disabled?:boolean;
}

export interface TableColumn{
    header:string, // string that will be display in column header,
    field:string, // Entity property name. is the id of that column. that is use to get data from that coulmn.
    sortable?:boolean,
    filtrable?:boolean,
    hide?:boolean,
    routable?:boolean,
    type:ColumnType,
    options: Option[],
    route?:string,
    btnValue?:string,
    dateFormat?:string
}

export enum ColumnType{
    SeqNum,
    EditBox,
    DropDown,
    CheckBox,
    Radio,
    Link,
    Barcode,
    SubLink,
    DetailNavBtn,
    DateTime
}
export interface ModifiedTableRow{
    row:any
}