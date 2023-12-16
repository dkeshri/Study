import { Component, OnInit } from '@angular/core';
import { faCoffee } from '@fortawesome/free-solid-svg-icons';
import { ColumnType, ModifiedTableRow, TableColumn } from 'src/app/theme/models/table-model';
import { Product } from '../Models/product.model';
import { ProductService } from '../service/product.service';
import { Format } from 'src/app/shared/models/app-constants';
interface City {
  name: string,
  code: string
}

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  barcode: string = '';
  products: Product[];
  cols: TableColumn[];
  isEditable: boolean = false;
  constructor(private productService:ProductService) {
    this.products = [];
    this.cols = [];
  }

  ngOnInit(): void {
    this.cols = [
      { field: 'id', header: 'Product Code', sortable: true, filtrable: true, type: ColumnType.Link },
      { field: 'barcode', header: 'Barcode', sortable: true, filtrable: true},
      { field: 'name', header: 'Name', sortable: false, filtrable: true },
      { field: 'unitId', header: 'Unit', sortable: false, filtrable: true },
      { field: 'costPrice', header: 'Cost Price', sortable: false, filtrable: true },
      { field: 'expireOn', header: 'Expire', sortable: false, filtrable: true,type: ColumnType.DateTime, dateFormat:Format.DATE }
    ] as TableColumn[];

    this.productService.getProducts().subscribe((products)=>{
      this.products = products;
    });
  }
  onEnterPress($event: any) {
    let barcode = this.barcode;
    console.log(barcode);
  }
  // clear(table: Table) {
  //   table.clear();
  // }
  onRowEditInit(event: ModifiedTableRow) {
    console.log(event);
  }

  onRowEditSave(event: ModifiedTableRow) {
    console.log(event)
  }
  onRowEditCancel(event: ModifiedTableRow) {
    console.log(event);
  }
}
