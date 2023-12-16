import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { UnitConversion, UnitConversionParam } from '../Models/unit-conversion.model';
import { UnitConversionService } from '../services/unit-conversion.service';

@Component({
  selector: 'app-create-unit-conversion',
  templateUrl: './create-unit-conversion.component.html',
  styleUrls: ['./create-unit-conversion.component.css']
})
export class CreateUnitConversionComponent implements OnInit {

  primariUnitLabel: string = '';
  secUnitLabel: string = '';
  conversionRate?:number;
  constructor(
    private messageService: MessageService,
    private dialogConfig:DynamicDialogConfig,
    private unitConversionService:UnitConversionService,
    private dialogRef:DynamicDialogRef
  ) { }

  ngOnInit(): void {
    let data = this.dialogConfig.data as UnitConversionParam;
    this.primariUnitLabel = data.primaryUnitText;
    this.secUnitLabel = data.secUnitText;
  }

  onSave() {
    let dialogConfigData = this.dialogConfig.data as UnitConversionParam;
    let data = {
      primaryId:dialogConfigData.primaryId,
      secId:dialogConfigData.secId,
      value:this.conversionRate,
      label: dialogConfigData.primaryUnitText + ' To ' + dialogConfigData.secUnitText
    } as UnitConversion;
    this.unitConversionService.createUnitConversion(data).subscribe(()=>{
      this.dialogRef?.close(data);
      this.messageService.add({ severity: 'success', summary: 'unit conversion added', detail: `1 ${dialogConfigData.primaryUnitText} = ${this.conversionRate} ${dialogConfigData.secUnitText}`, life: 3000 });
    });
  }


}
