import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-alert-error',
  templateUrl: './alert-error.component.html',
  styleUrls: ['./alert-error.component.css']
})
export class AlertErrorComponent implements OnInit {
  @Input() errorTitle: string;
  @Input() errorMessages: string[] = [];

  ngOnInit() {
  }

}
