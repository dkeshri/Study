import { NgModule } from '@angular/core';
import {MatButtonModule} from '@angular/material/button';
import {MatCardModule} from '@angular/material/card';
import {MatIconModule} from '@angular/material/icon';
import {MatMenuModule} from '@angular/material/menu';
import {MatDividerModule} from '@angular/material/divider';
import {MatTabsModule} from '@angular/material/tabs';
const MATERIAL_MODULES = [
  MatButtonModule,
  MatCardModule,
  MatIconModule,
  MatMenuModule,
  MatDividerModule,
  MatTabsModule
]

@NgModule({
  imports: [MATERIAL_MODULES],
  exports:[MATERIAL_MODULES]
})
export class MaterialModule { }
