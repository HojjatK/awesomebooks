import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ReactiveFormsModule, FormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CategoryArea } from '../../models/category-area.model';
import { Category } from '../../models/category.model';
import { CategoryAreaService } from '../../services/category-area.service';
import { CategoryService } from '../../services/category.service';

@Component({
  selector: 'app-new-category',
  templateUrl: './new-category.component.html',
  styleUrls: ['./new-category.component.css']
})
export class NewCategoryComponent implements OnInit {
  newCategoryForm: FormGroup;
  name: FormControl;
  area: FormControl;
  description: FormControl;
  selectedAreaId: number;
  categoryAreas: CategoryArea[];
  errorMessage: string;

  constructor(
    private location: Location,
    private categoryAreaService: CategoryAreaService,
    private categoryService: CategoryService,
    private toastr: ToastrService,
    fb: FormBuilder) {
    this.newCategoryForm = fb.group({
      'name': ['', [Validators.required, Validators.maxLength(50)]],
      'area': ['', [Validators.required]],
      'description': ['', Validators.maxLength(1024)]
    });
    this.name = this.newCategoryForm.controls['name'] as FormControl;
    this.area = this.newCategoryForm.controls['area'] as FormControl;
    this.description = this.newCategoryForm.controls['description'] as FormControl;
  }

  ngOnInit() {    
    this.categoryAreaService.getAll()
      .subscribe(data => {
        console.log('Category areas loaded');
        this.categoryAreas = data as CategoryArea[];
      });
  }

  goBack = () => {
    this.location.back();
  }

  onSubmit = () => {
    if (this.newCategoryForm.valid) {
      if (this.selectedAreaId == undefined || this.selectedAreaId == 0) {
        this.errorMessage = "Area* is required";
        return;
      }

      var newCategory = new Category();
      newCategory.name = this.newCategoryForm.value["name"];
      newCategory.description = this.newCategoryForm.value["description"];      
      newCategory.areaId = this.selectedAreaId;      
      newCategory.areaName = this.getAreaName(this.selectedAreaId);

      this.categoryService.create(newCategory).subscribe(
        (data) => {
          this.errorMessage = '';
          this.toastr.success('Category created successfully')
          this.location.back();
        });
    }
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
