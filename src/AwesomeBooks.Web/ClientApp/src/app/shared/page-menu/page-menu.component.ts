import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import {
  ReactiveFormsModule,
  FormsModule,
  FormGroup,
  FormControl,
  Validators,
  FormBuilder
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-page-menu',
  templateUrl: './page-menu.component.html',
  styleUrls: ['./page-menu.component.css']
})
export class PageMenuComponent implements OnInit {
  isExtToolbarVisible: boolean = false;
  uploadForm: FormGroup;  

  constructor(
    private toastService: ToastrService,
    private fb: FormBuilder) {   
  }

  @Input() newText: string;
  @Input() uploadFile: File;
  @Input() downloadFileUrl: string;

  @Output() newSelected: EventEmitter<void> = new EventEmitter<void>();
  @Output() uploadSelected: EventEmitter<File> = new EventEmitter<File>();

  ngOnInit() {
    this.uploadForm = this.fb.group({
      'uploadFileControl': ['', null],
    });
  }

  onToggleMore = () => {
    this.isExtToolbarVisible = !this.isExtToolbarVisible;
  }

  onNewSelected = () => {
    this.newSelected.emit();
  }

  public uploadFileChange(files: any) {
    this.uploadFile = files[0]
  }

  onSubmitUpload = () => {
    if (this.uploadFile == undefined || this.uploadFile.name == undefined) {
      this.toastService.warning('Please, choose the file to upload and try again.');
      return;
    }
    this.uploadSelected.emit(this.uploadFile);
  }
}
