import { NgModule } from '@angular/core';
import {MatLegacyButtonModule as MatButtonModule} from '@angular/material/legacy-button';
import {MatLegacyCardModule as MatCardModule} from '@angular/material/legacy-card';
import {MatIconModule} from '@angular/material/icon';
import {MatLegacyMenuModule as MatMenuModule} from '@angular/material/legacy-menu';
import {MatDividerModule} from '@angular/material/divider';
import {MatLegacyTabsModule as MatTabsModule} from '@angular/material/legacy-tabs';
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
