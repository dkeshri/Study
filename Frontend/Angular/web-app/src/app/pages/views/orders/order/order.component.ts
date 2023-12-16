import { Barcode, BarcodeCreate } from './../../../../core/model/barcode-model';
import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { Table } from 'primeng/table';
import { BarcodeService } from 'src/app/services/barcode.service';
import { ColumnType, ModifiedTableRow, Option, TableColumn } from 'src/app/theme/models/table-model';
import { ActivatedRoute, Router } from '@angular/router';
import { UnitService } from 'src/app/services/unit.service';
import { ProductService } from '../../products/service/product.service';
import { Order, OrdersSummary } from '../Model/order-model';
import { Product } from '../../products/Models/product.model';
import { OrderService } from 'src/app/services/order.service';
import { CustomerService } from '../../customers/services/customer.service';
import { Customer } from '../../customers/models/customer.model';
import { UnitConversion } from '../../units/Models/unit-conversion.model';
import { UnitConversionService } from '../../units/services/unit-conversion.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  barcode: string = '';
  customerId!: number;
  customer?: Customer;
  customerBarcode: string = '';
  columns: TableColumn[] = [
    { field: 'barcode', header: 'Common.Barcode', type: ColumnType.Barcode },
    { field: 'procuctName', header: 'Table.Column.Header.ProductName' },
    { field: 'unitId', header: 'Common.Unit', type: ColumnType.DropDown },
    { field: 'price', header: 'Common.Price', type: ColumnType.EditBox },
    { field: 'quantity', header: 'Common.Quantity', type: ColumnType.EditBox },
    { field: 'amount', header: 'Common.Amount' }
  ] as TableColumn[];
  orderItems: Order[] = [];
  isEditable: boolean = true;
  @Output()
  RowEditSave: EventEmitter<ModifiedTableRow> = new EventEmitter<ModifiedTableRow>();

  ColumnType = ColumnType;
  globalFilterFields: string[] = [];
  clonedValue: { [s: string]: any; } = {};

  saveImage!: string;
  cancelImage!: string;
  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private barcodeService: BarcodeService,
    private router: Router,
    private unitService: UnitService,
    private productService: ProductService,
    private orderService: OrderService,
    private customerService: CustomerService,
    private unitconversionService: UnitConversionService,
    private route: ActivatedRoute
  ) {

  }

  @ViewChild('dt', { read: Table })
  table!: Table;
  @ViewChild('barcodeSearch')
  barcodeSearch!: ElementRef;

  private saveBarcodeReq = {
    code: 'Save',
    width: 220,
    height: 50,
    includeLabel: true
  } as BarcodeCreate;
  private cancelBarcodeReq = {
    code: 'Cancel',
    width: 220,
    height: 50,
    includeLabel: true
  } as BarcodeCreate;
  unitOption!: Option[];
  unitConversions!: UnitConversion[];
  ngOnInit(): void {
    this.route.queryParamMap
      .subscribe((params) => {
        const barcode = params.get('barcode');
        if(barcode){
          this.customerBarcode = barcode;
          this.getCustomerDetail(this.customerBarcode);
        }
      }
      );
    this.barcodeService.getBarcode(this.saveBarcodeReq).subscribe((barcode: Barcode) => {
      this.saveImage = "data:image/png;base64," + barcode.image;
    });
    this.barcodeService.getBarcode(this.cancelBarcodeReq).subscribe((barcode: Barcode) => {
      this.cancelImage = "data:image/png;base64," + barcode.image;
    });
    this.unitconversionService.getUnitConversions().subscribe((unitconversions) => {
      this.unitConversions = unitconversions;
    })
    this.unitService.getUnitOptions().subscribe((options) => {
      this.unitOption = options;
    });
    this.globalFilterFields = this.columns.map((column) => {
      return column.field;
    });
  }
  onRowEditInit(row: Order) {
    this.clonedValue[row.barcode] = { ...row };
  }

  onRowEditSave(row: Order) {
    if (this.isValidRow()) {
      let price = row.price;
      if (row.unitId === row.secUnitId) {
        price = row.price / row.unitConversionRate;
      }
      row.amount = price * row.quantity;
      let modifiedTableRow: ModifiedTableRow = {
        row: row
      };
      delete this.clonedValue[row.barcode];
      this.RowEditSave.emit(modifiedTableRow);
    } else {
      alert('Invalid data')
    }

  }
  onRowEditCancel(row: any) {
    let prevValue = this.clonedValue[row[this.columns[0].field]];
    // Iterate through the object
    for (const key in prevValue) {
      if (prevValue.hasOwnProperty(key)) {
        row[key] = prevValue[key];
      }
    }
    delete this.clonedValue[row[this.columns[0].field]];
  }
  isValidRow(): boolean {
    return true
  }
  private getCustomerDetail(customerBarcode:string){
    this.customerService.getCustomerByBarcode(customerBarcode).subscribe((customer) => {
      this.customerId = customer.id;
      this.customer = customer
    });
  }
  onCustomerBarcodeEnterPress($event: Event) {
    let customerBarcode = this.customerBarcode;
    this.getCustomerDetail(customerBarcode);
  }
  onProductBarcodeEnterPress($event: any) {
    // if product already in the order list then Update product.
    let existingOrder = this.orderItems.find(item => item.barcode === this.barcode);

    if (existingOrder) {
      if (existingOrder.unitId !== existingOrder.secUnitId) {
        ++existingOrder.quantity;
        existingOrder.amount = existingOrder.quantity * existingOrder.price;
      } else {
        this.messageService.add({ severity: 'warn', summary: 'Product ' + existingOrder.procuctName, detail: 'Please increase the quantity', life: 5000 });
      }
      this.barcode = '';
    }

    if (existingOrder === undefined) {
      // Add the new Product in the list.
      this.addNewProductToOrderList();
    }

  }

  private addNewProductToOrderList() {
    this.productService.getProductByBarcode(this.barcode).subscribe((product: Product) => {
      let unitOptions = this.unitOption.filter(unit => {
        if (unit.value === product.unitId || unit.value === product.secUnitId) {
          return true;
        } else {
          return false;
        }
      });
      let conversionRate = this.unitConversions.find(item => item.id === product.unitConversionId)?.value ?? 1
      let row = {
        barcode: this.barcode,
        procuctName: product.name,
        productId: product.id,
        price: product.price,
        quantity: 1,
        amount: product.price,
        unitId: product.unitId,
        secUnitId: product.secUnitId,
        unitConversionRate: conversionRate,
        units: unitOptions
      } as Order;

      this.orderItems = [row, ...this.orderItems];
      this.barcode = '';
    });
  }
  onRowDelete(row: Order) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the selected products?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.orderItems = this.orderItems.filter((data) => {
          if (data.barcode == row.barcode) {
            return false;
          }
          return true;
        });
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Products Deleted', life: 3000 });
      }
    });


  }
  private getOrderSummary(): OrdersSummary {
    let ordersSummary = {} as OrdersSummary;
    ordersSummary.isActive = true;
    ordersSummary.isCash = true;
    ordersSummary.customerId = this.customerId;
    ordersSummary.orders = this.orderItems;
    let totalAmount = 0;
    let orderDescription = [] as string[];
    let orderDescCounter = 3;
    this.orderItems.forEach((item) => {
      totalAmount = totalAmount + item.amount;
      if(orderDescCounter>=0){
        if(orderDescCounter === 0){
          orderDescription.push('...');
        }else{
          orderDescription.push(item.procuctName);
        }
        --orderDescCounter;
      }
    });
    ordersSummary.ordersSummaryDesc = orderDescription.toString();
    ordersSummary.totalAmount = totalAmount;
    return ordersSummary;
  }
  onSave() {
    let ordersSummary = this.getOrderSummary();
    this.orderService.createOrdersSummary(ordersSummary).subscribe((ordersSummary: OrdersSummary) => {
      this.messageService.add({ severity: 'success', summary: 'Successfull', detail: 'Orders summary Created', life: 3000 });
    });
  }
  onCancel() {
    this.router.navigate(['']);
  }
}
