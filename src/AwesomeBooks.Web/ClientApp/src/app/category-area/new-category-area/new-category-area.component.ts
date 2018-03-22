import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common'; 
import { ReactiveFormsModule, FormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CategoryArea } from '../../models/category-area.model';
import { CategoryAreaService } from '../../services/category-area.service';

@Component({
  selector: 'app-new-category-area',
  templateUrl: './new-category-area.component.html',
  styleUrls: ['./new-category-area.component.css']
})
export class NewCategoryAreaComponent implements OnInit {
  newCategoryAreaForm: FormGroup;
  name: FormControl;
  description: FormControl;
  errorMessage: string;

  constructor(
    private location: Location,
    private categoryAreaService: CategoryAreaService,
    private toastr: ToastrService,
    fb: FormBuilder) {
    this.newCategoryAreaForm = fb.group({
      'name': ['', [Validators.required, Validators.maxLength(50)]],
      'description': ['', Validators.maxLength(1024)]
    });
    this.name = this.newCategoryAreaForm.controls['name'] as FormControl;
    this.description = this.newCategoryAreaForm.controls['description'] as FormControl;
  }

  ngOnInit() {
  }

  goBack = () => {
    this.location.back();
  }

  onSubmit = () => {
    if (this.newCategoryAreaForm.valid) {
      var newCategoryArea = new CategoryArea();
      newCategoryArea.name = this.newCategoryAreaForm.value["name"];
      newCategoryArea.description = this.newCategoryAreaForm.value["description"];
      this.categoryAreaService.create(newCategoryArea).subscribe(
        (data) => {
          this.errorMessage = '';
          this.toastr.success('Area created successfully');
          this.location.back();
        });        
    }
  }
}
