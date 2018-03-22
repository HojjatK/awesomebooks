import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { Location } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ReactiveFormsModule, FormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { CategoryArea } from '../../models/category-area.model';
import { CategoryAreaService } from '../../services/category-area.service';

@Component({
  selector: 'app-edit-category-area',
  templateUrl: './edit-category-area.component.html',
  styleUrls: ['./edit-category-area.component.css']
})
export class EditCategoryAreaComponent implements OnInit {
  areaId: number;  
  editCategoryAreaForm: FormGroup;
  name: FormControl;
  description: FormControl;
  errorMessage: string;

  constructor(private location: Location,
              private route: ActivatedRoute,
              private categoryAreaService: CategoryAreaService,
              private toastr: ToastrService,
              fb: FormBuilder) {
    this.route.params.subscribe(params =>
    {
      this.areaId = params['id'];
    });
    this.editCategoryAreaForm = fb.group({
      'name': ['', [Validators.required, Validators.maxLength(50)]],
      'description': ['', Validators.maxLength(1024)]
    });
    this.name = this.editCategoryAreaForm.controls['name'] as FormControl;
    this.description = this.editCategoryAreaForm.controls['description'] as FormControl;
  }

  ngOnInit() {
    this.categoryAreaService.get(this.areaId).subscribe(
      data => {
        this.name.setValue(data.name);
        this.description.setValue(data.description);
      });
  }

  onSubmit = () => {
    if (this.editCategoryAreaForm.valid) {
      var model = new CategoryArea();
      model.id = this.areaId;
      model.name = this.editCategoryAreaForm.value["name"];
      model.description = this.editCategoryAreaForm.value["description"];
      this.categoryAreaService.update(model).subscribe(
        (data) => {
          this.errorMessage = '';
          this.toastr.success(`Area updated successfully.`);
          this.location.back();
        });
    }
  }

  goBack = () => {
    this.location.back();
  }
}
