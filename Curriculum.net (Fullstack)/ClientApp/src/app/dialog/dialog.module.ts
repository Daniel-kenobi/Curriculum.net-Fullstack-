import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DialogComponent } from './dialog/dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogService } from './dialogservice.service';
import { PdfViewerModule } from 'ng2-pdf-viewer';


@NgModule({
  declarations: [
    DialogComponent
  ],
  imports: [
    CommonModule,
    MatDialogModule,
    PdfViewerModule
  ],
  entryComponents: [
    DialogComponent
  ],
  providers: [DialogService]
})
export class DialogModule { }
