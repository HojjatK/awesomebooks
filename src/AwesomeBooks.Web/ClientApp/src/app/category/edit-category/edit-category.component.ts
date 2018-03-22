import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { Location } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ReactiveFormsModule, FormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { CategoryArea } from '../../models/category-area.model';
import { Category } from '../../models/category.model';
import { CategoryAreaService } from '../../services/category-area.service';
import { CategoryService } from '../../services/category.service';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit {
  categoryId: number;
  editCategoryForm: FormGroup;
  name: FormControl;
  area: FormControl;
  description: FormControl;
  selectedAreaId: number;
  categoryAreas: CategoryArea[];
  errorMessage: string;

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private categoryAreaService: CategoryAreaService,
    private categoryService: CategoryService,
    private toastr: ToastrService,
    fb: FormBuilder) {
    this.route.params.subscribe(params => {
      this.categoryId = params['id'];      
    });
    this.editCategoryForm = fb.group({
      'name': ['', [Validators.required, Validators.maxLength(50)]],
      'area': ['', [Validators.required]],
      'description': ['', Validators.maxLength(1024)]
    });
    this.name = this.editCategoryForm.controls['name'] as FormControl;
    this.area = this.editCategoryForm.controls['area'] as FormControl;
    this.description = this.editCategoryForm.controls['description'] as FormControl;
  }

  ngOnInit() {
    if (this.categoryId <= 0) {
      return;
    }

    this.categoryAreaService.getAll()
      .subscribe(data => {
        this.categoryAreas = data as CategoryArea[];

        this.categoryService.get(this.categoryId).subscribe(
          data => {
            this.name.setValue(data.name);
            this.area.setValue(data.areaId);
            this.description.setValue(data.description);
          });
      });
  }

  onSubmit = () => {
    if (this.editCategoryForm.valid) {

      if (this.selectedAreaId == undefined || this.selectedAreaId == 0) {
        this.errorMessage = "Area* is required";
        return;
      }

      var model = new Category();
      model.id = this.categoryId;
      model.name = this.editCategoryForm.value["name"];
      model.areaId = this.editCategoryForm.value["area"];
      model.description = this.editCategoryForm.value["description"];
      model.areaName = this.getAreaName(this.selectedAreaId);

      this.categoryService.update(model).subscribe(
        (data) => {
          this.errorMessage = '';
          this.toastr.success(`Category updated successfully`);
          this.location.back();
        });
    }
  }

  goBack = () => {
    this.location.back();
  }

  getAreaName = (areaId: number): string => {
    let areaName = '';
    for (var i = 0; i < this.categoryAreas.length; i++) {
      if (this.categoryAreas[i].id == areaId) {
        areaName = this.categoryAreas[i].name;
      }
    }
    return areaName;
  }
}
