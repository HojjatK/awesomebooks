import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ConfirmDialogComponent } from './../confirm-dialog/confirm-dialog.component';
import { MatDialog, MatDialogRef } from '@angular/material';

@Injectable()
export class DialogService {

  constructor(private dialog: MatDialog) {
  }

  public confirm(title: string, message: string): Observable<boolean> {
    let dialogRef: MatDialogRef<ConfirmDialogComponent>;
    dialogRef = this.dialog.open(ConfirmDialogComponent);

    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.message = message;
    return dialogRef.afterClosed();
  }
}
