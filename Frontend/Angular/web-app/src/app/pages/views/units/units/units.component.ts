import { UnitService } from './../../../../services/unit.service';
import { Component, OnInit } from '@angular/core';
import { Unit } from 'src/app/services/models/unit.model';
import { ColumnType, ModifiedTableRow, TableColumn } from 'src/app/theme/models/table-model';

@Component({
  selector: 'app-units',
  templateUrl: './units.component.html',
  styleUrls: ['./units.component.css']
})
export class UnitsComponent implements OnInit {
  cols!: TableColumn[];
  isEditable: boolean = false;
  constructor(private unitService:UnitService) { }

  units:Unit[] = [];
  ngOnInit(): void {
    
    this.cols = [
      { field: 'id', header: 'Unit Code', sortable: true, filtrable: true, type: ColumnType.Link },
      { field: 'name', header: 'Name', sortable: true, filtrable: false },
      { field: 'shortName', header: 'Short Name', sortable: false, filtrable: true },
      { field: 'user', header: 'User', sortable: false, filtrable: true },
      { field: 'isActive', header: 'Active', sortable: false, filtrable: true }
    ] as TableColumn[];

    this.unitService.getUnits().subscribe((units)=>{
      this.units = units;
    });

  }

  onRowEditCancel(event: ModifiedTableRow) {
    console.log(event);
  }
  onRowEditSave(event: ModifiedTableRow) {
    console.log(event)
  }
}