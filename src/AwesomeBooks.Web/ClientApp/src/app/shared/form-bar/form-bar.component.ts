import { Component, OnInit, Input } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-form-bar',
  templateUrl: './form-bar.component.html',
  styleUrls: ['./form-bar.component.css']
})
export class FormBarComponent implements OnInit {

  constructor(private location: Location) { }

  @Input() menuText: string;

  ngOnInit() {
  }

  goBack() {
    this.location.back();
  }
}
