import { Component, OnInit } from '@angular/core';
import { GeoRegionService } from 'src/app/services/geo-region.service';
import { City } from 'src/app/services/models/geo-region.model';
import { ColumnType, TableColumn } from 'src/app/theme/models/table-model';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.css']
})
export class CitiesComponent implements OnInit {

  cols!: TableColumn[];
  isEditable: boolean = false;
  cities!: City[];
  constructor(private geoRegionService: GeoRegionService) { }

  ngOnInit(): void {
    this.cols = [
      { field: 'id', header: 'Country Id', sortable: true, filtrable: true, type: ColumnType.Link },
      { field: 'name', header: 'Name', sortable: true, filtrable: true },
      { field: 'stateCode', header: 'State Code', sortable: true, filtrable: true },
      { field: 'countryCode', header: 'Country Code', sortable: true, filtrable: true },
      { field: 'latitude', header: 'Longitude', sortable: true, filtrable: true },
      { field: 'longitude', header: 'Longitude', sortable: true, filtrable: true }
    ] as TableColumn[];
    this.geoRegionService.getCities().subscribe((data)=>{
      this.cities = data;
    })
  }

}
