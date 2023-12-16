import { ColumnType, ModifiedTableRow } from './../../../theme/models/table-model';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TableColumn } from 'src/app/theme/models/table-model';
import { Format, TableProperty } from '../../models/app-constants';

@Component({
  selector: 'app-data-table',
  templateUrl: './data-table.component.html',
  styleUrls: ['./data-table.component.css']
})
export class DataTableComponent implements OnInit {

  @Input()
  columns: TableColumn[] = [] as TableColumn[];
  @Input()
  value: any[] = [] as any[];
  @Input()
  isEditable: boolean = false;
  @Input()
  sortOrder:number = TableProperty.SORT_ASC;
  @Input()
  defaultSortColIndex:number = 0;
  @Output()
  RowEditSave: EventEmitter<ModifiedTableRow> = new EventEmitter<ModifiedTableRow>();

  ColumnType = ColumnType;
  globalFilterFields: string[] = [];
  clonedValue: { [s: string]: any; } = {};

  constructor() {

  }


  ngOnInit(): void {
    this.globalFilterFields = this.columns.map((column) => {
      return column.field;
    });
  }
  onRowEditInit(row: any) {
    this.clonedValue[row[this.columns[0].field]] = {...row};
  }

  onRowEditSave(row: any) {
    if(this.isValidRow()){
      let modifiedTableRow: ModifiedTableRow = {
        row: row
      };
      delete this.clonedValue[row[this.columns[0].field]];
      this.RowEditSave.emit(modifiedTableRow);
    }else{
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
  isValidRow():boolean{
    return true
  }
}
