import { RouteAction } from './../../../../shared/models/app-constants';
import { Unit } from 'src/app/services/models/unit.model';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { CreateUnit } from './../../../../services/models/unit.model';
import { UnitService } from './../../../../services/unit.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-create-unit',
  templateUrl: './create-unit.component.html',
  styleUrls: ['./create-unit.component.css']
})
export class CreateUnitComponent implements OnInit {
  unitForm: FormGroup = this.formBuilder.group({
    name: ['', [Validators.required]],
    shortName: [''],
    isActive: [true]
  });
  unitId: string = '';
  showUpdatebtn: boolean = false;
  showDeletebtn: boolean = false;
  constructor(private formBuilder: FormBuilder,
    private unitService: UnitService,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private dialogRef: DynamicDialogRef
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.unitId = params['unitId'];
      if (this.unitId && (this.unitId != RouteAction.ADD)) {
        this.showUpdatebtn = true;
        this.showDeletebtn = true;
        this.getUnitDetail(this.unitId);
      } else {
        // add case
        this.showDeletebtn = false;
        this.showUpdatebtn = false;
      }
    });
  }
  private getUnitDetail(unitId: string) {
    this.unitService.getUnit(unitId).subscribe((unit) => {
      this.setFormOnGetUnit(unit);
    });
  }
  private setFormOnGetUnit(unit: Unit) {
    this.unitForm.controls['name'].setValue(unit.name);
    this.unitForm.controls['shortName'].setValue(unit.shortName);
    this.unitForm.controls['isActive'].setValue(unit.isActive);
  }
  get unitName() {
    return this.unitForm.get('name') as FormControl;
  }
  onSave() {
    let unit = {} as CreateUnit;
    unit.name = this.unitForm.controls['name'].value;
    unit.shortName = this.unitForm.controls['shortName'].value;
    unit.isActive = this.unitForm.controls['isActive'].value;
    unit.userId = 1;
    this.unitService.createUnit(unit).subscribe((data: Unit) => {
      this.dialogRef?.close(data.id);
      this.messageService.add({ severity: 'success', summary: 'Unit Created Successfully', detail: data.name, life: 3000 });
    })
  }
  onUpdate() {

  }
  onDelete() {

  }
}
