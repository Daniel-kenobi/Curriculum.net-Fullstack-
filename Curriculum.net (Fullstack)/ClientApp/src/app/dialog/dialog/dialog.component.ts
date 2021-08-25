import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html'
})
export class DialogComponent implements OnInit {

  constructor(
    private dialogRef: MatDialogRef<DialogComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void { }

  fnc_confirmar() {
    this.dialogRef.close();
  }

  fnc_cancelar() {
    this.dialogRef.close();
  }

}
