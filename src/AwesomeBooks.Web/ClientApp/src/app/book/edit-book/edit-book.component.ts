import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from "@angular/router";
import { ReactiveFormsModule, FormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Category } from '../../models/category.model';
import { CategoryArea } from '../../models/category-area.model';
import { Book } from '../../models/book.model';
import { CategoryService } from '../../services/category.service';
import { CategoryAreaService } from '../../services/category-area.service';
import { BookService } from '../../services/book.service';

@Component({
  selector: 'app-edit-book',
  templateUrl: './edit-book.component.html',
  styleUrls: ['./edit-book.component.css']
})
export class EditBookComponent implements OnInit {
  bookId: number;
  editBookForm: FormGroup;
  name: FormControl;
  area: FormControl;
  category: FormControl;
  year: FormControl;
  authors: FormControl;
  imageUri: FormControl;
  amazonUri: FormControl;
  downloadUri: FormControl;
  reflection: FormControl;
  selectedAreaId: number;
  selectedCategoryId: number;
  selectedImageUrl: string = '';
  selectedAmazonUrl: string = '';
  categoryAreas: CategoryArea[] = [];
  allCategories: Category[] = [];
  categories: Category[] = [];
  errorMessage: string;

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private categoryAreaService: CategoryAreaService,
    private categoryService: CategoryService,
    private bookService: BookService,
    private toastr: ToastrService,
    fb: FormBuilder) {
    this.route.params.subscribe(params => {
      this.bookId = params['id'];
    });
    this.editBookForm = fb.group({
      'name': ['', [Validators.required, Validators.maxLength(255)]],
      'year': ['', [Validators.required, Validators.max(9999), Validators.min(1000)]],
      'authors': ['', Validators.maxLength(150)],
      'area': ['', Validators.required],
      'category': ['', Validators.required],
      'imageUri': ['', Validators.maxLength(1024)],
      'amazonUri': ['', Validators.maxLength(1024)],
      'downloadUri': ['', Validators.maxLength(1024)],
      'reflection': ['', Validators.maxLength(8000)]
    });
    this.name = this.editBookForm.controls['name'] as FormControl;
    this.area = this.editBookForm.controls['area'] as FormControl;
    this.category = this.editBookForm.controls['category'] as FormControl;
    this.year = this.editBookForm.controls['year'] as FormControl;
    this.authors = this.editBookForm.controls['authors'] as FormControl;
    this.category = this.editBookForm.controls['category'] as FormControl;
    this.imageUri = this.editBookForm.controls['imageUri'] as FormControl;
    this.amazonUri = this.editBookForm.controls['amazonUri'] as FormControl;
    this.downloadUri = this.editBookForm.controls['downloadUri'] as FormControl;
    this.reflection = this.editBookForm.controls['reflection'] as FormControl;
  }

  ngOnInit() {
    if (this.bookId <= 0) {
      return;
    }
    
    this.categoryAreaService.getAll()
      .subscribe(areas => {
        this.categoryAreas = areas as CategoryArea[];

        this.categoryService.getAll()
          .subscribe(categories => {
            this.allCategories = categories as Category[];

            this.bookService.get(this.bookId)
              .subscribe(book => {
                this.name.setValue(book.name);
                this.area.setValue(book.areaId);
                this.populateCategories(this.selectedAreaId);
                this.category.setValue(book.categoryId);                
                this.year.setValue(book.publishYear);
                this.authors.setValue(book.authors);
                this.imageUri.setValue(book.imageUri);
                this.amazonUri.setValue(book.amazonUri);
                this.downloadUri.setValue(book.contentUri);
                this.reflection.setValue(book.reflection);
              });            
          });
      });
  }

  populateCategories = (areaId: number) => {      
    this.categories.length = 0;
    for (let c of this.allCategories) {
      if (c.areaId == areaId) {
        this.categories.push(c);
      }
    }
  }

  categoryAreaChange = () => {
    let areaId = this.selectedAreaId;
    this.populateCategories(areaId);

    this.selectedCategoryId = undefined;
    if (areaId > 0 && this.categories.length > 0) {
      this.selectedCategoryId = this.categories[0].id;
    }
  }

  goBack = () => {
    this.location.back();
  }

  onSubmit = () => {
    if (this.editBookForm.valid) {
      if (this.selectedAreaId == undefined || this.selectedAreaId == 0) {
        this.errorMessage = "Area* is required";
        return;
      }

      if (this.selectedCategoryId == undefined || this.selectedCategoryId == 0) {
        this.errorMessage = "Category* is required";
        return;
      }

      var model = new Book();
      model.id = this.bookId;
      model.name = this.editBookForm.value["name"];
      model.areaId = this.editBookForm.value["area"];
      model.areaName = this.getAreaName(model.areaId);
      model.categoryId = this.editBookForm.value["category"];
      model.categoryName = this.getCategoryName(model.categoryId);
      model.publishYear = this.editBookForm.value["year"];
      model.authors = this.editBookForm.value["authors"];
      model.imageUri = this.editBookForm.value["imageUri"];
      model.amazonUri = this.editBookForm.value["amazonUri"];
      model.contentUri = this.editBookForm.value["downloadUri"];
      model.reflection = this.editBookForm.value["reflection"];

      this.bookService.update(model).subscribe(
        (data) => {
          this.errorMessage = '';
          this.toastr.success('Book updated successfully')
          this.location.back();
        });
    }
  }

  getAreaName = (areaId: number) => {
    let areaName = '';
    for (var i = 0; i < this.categoryAreas.length; i++) {
      if (this.categoryAreas[i].id == areaId) {
        areaName = this.categoryAreas[i].name;
      }
    }
    return areaName;
  }

  getCategoryName = (categoryId: number) => {
    let categoryName = '';
    for (var i = 0; i < this.categories.length; i++) {
      if (this.categories[i].id == categoryId) {
        categoryName = this.categories[i].name;
      }
    }
    return categoryName;
  }
}
