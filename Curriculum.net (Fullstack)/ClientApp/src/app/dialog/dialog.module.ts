import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DialogComponent } from './dialog/dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogService } from './dialogservice.service';



@NgModule({
  declarations: [
    DialogComponent
  ],
  imports: [
    CommonModule,
    MatDialogModule,
  ],
  entryComponents: [
    DialogComponent
  ],
  providers: [DialogService]
})
export class DialogModule { }
