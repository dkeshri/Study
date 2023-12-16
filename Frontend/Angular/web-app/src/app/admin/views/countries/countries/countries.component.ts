import { Component, OnInit } from '@angular/core';
import { GeoRegionService } from 'src/app/services/geo-region.service';
import { Country } from 'src/app/services/models/geo-region.model';
import { ColumnType, TableColumn } from 'src/app/theme/models/table-model';

@Component({
  selector: 'app-countries',
  templateUrl: './countries.component.html',
  styleUrls: ['./countries.component.css']
})
export class CountriesComponent implements OnInit {

  cols!: TableColumn[];
  isEditable: boolean = false;
  countries!: Country[];
  constructor(private geoRegionService: GeoRegionService) { }

  ngOnInit(): void {
    this.cols = [
      { field: 'id', header: 'Country Id', sortable: true, filtrable: true, type: ColumnType.Link },
      { field: 'name', header: 'Name', sortable: true, filtrable: true },
      { field: 'capital', header: 'Capital', sortable: true, filtrable: true },
      { field: 'iso2', header: 'ISO2', sortable: true, filtrable: true },
      { field: 'iso3', header: 'ISO3', sortable: true, filtrable: true },
      { field: 'currency', header: 'Currency', sortable: true, filtrable: true },
      { field: 'currencySymbol', header: 'Currency Symbol', sortable: true, filtrable: true }
    ] as TableColumn[];
    this.geoRegionService.getCountries().subscribe((data)=>{
      this.countries = data;
      //console.log(this.countries);
    });
  }

}
