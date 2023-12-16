import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { UnitService } from './../../../../services/unit.service';
import { Option } from 'src/app/theme/models/table-model';
import { Component, OnInit } from '@angular/core';
@Component({
  selector: 'app-unit-details',
  templateUrl: './unit-details.component.html',
  styleUrls: ['./unit-details.component.css']
})
export class UnitDetailsComponent implements OnInit {
  secUnitOptions: Option[] = [];
  constructor(private unitService:UnitService) { }
  trash = faTrash;
  trashStyles = { 
    color: 'red'
   };
  ngOnInit(): void {
   
  }
  onSubmit(){

  }

}
