import { MenubarModule } from 'primeng/menubar';
import { NgModule } from '@angular/core';
import {InputTextModule} from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import {CardModule} from 'primeng/card';
import {DividerModule} from 'primeng/divider';
import {DropdownModule} from 'primeng/dropdown';
import {TableModule} from 'primeng/table';
import {MessagesModule} from 'primeng/messages';
import {DialogModule} from 'primeng/dialog';
import {AvatarModule} from 'primeng/avatar';
import {MenuModule} from 'primeng/menu';
import {CalendarModule} from 'primeng/calendar';
import {CheckboxModule} from 'primeng/checkbox';
import {DialogService, DynamicDialogConfig, DynamicDialogModule, DynamicDialogRef} from 'primeng/dynamicdialog';
import { TabViewModule } from 'primeng/tabview';
@NgModule({
  declarations: [],
  imports: [
  ],
  exports:[
    InputTextModule,
    ButtonModule,
    CardModule,
    DividerModule,
    DropdownModule,
    MenubarModule,
    TableModule,
    MessagesModule,
    DialogModule,
    AvatarModule,
    MenuModule,
    CalendarModule,
    CheckboxModule,
    DynamicDialogModule,
    TabViewModule
  ],
  providers:[DialogService,DynamicDialogRef,DynamicDialogConfig]
})
export class PrimengModule { }
