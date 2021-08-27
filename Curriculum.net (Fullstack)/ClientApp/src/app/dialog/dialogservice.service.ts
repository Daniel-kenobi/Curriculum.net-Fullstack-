import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponent } from './dialog/dialog.component';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog) { }

  fnc_open_dialog() {
    return this.dialog.open(DialogComponent, {
      panelClass: 'container'
    });
  }
}
