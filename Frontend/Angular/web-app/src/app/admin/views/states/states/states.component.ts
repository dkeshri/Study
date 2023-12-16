import { Component, OnInit } from '@angular/core';
import { GeoRegionService } from 'src/app/services/geo-region.service';
import { State } from 'src/app/services/models/geo-region.model';
import { ColumnType, TableColumn } from 'src/app/theme/models/table-model';

@Component({
  selector: 'app-states',
  templateUrl: './states.component.html',
  styleUrls: ['./states.component.css']
})
export class StatesComponent implements OnInit {

  cols!: TableColumn[];
  isEditable: boolean = false;
  states!: State[];
  constructor(private geoRegionService: GeoRegionService) { }

  ngOnInit(): void {
    this.cols = [
      { field: 'id', header: 'State Id', sortable: true, filtrable: true, type: ColumnType.Link },
      { field: 'name', header: 'Name', sortable: true, filtrable: true },
      { field: 'stateCode', header: 'State Code', sortable: true, filtrable: true },
      { field: 'type', header: 'Type', sortable: true, filtrable: true },
      { field: 'countryCode', header: 'Country Code', sortable: true, filtrable: true },
      { field: 'latitude', header: 'Latitude', sortable: true, filtrable: true },
      { field: 'longitude', header: 'Longitude', sortable: true, filtrable: true }
    ] as TableColumn[];
    this.geoRegionService.getStates().subscribe((data)=>{
      this.states = data;
    });
  }

}
