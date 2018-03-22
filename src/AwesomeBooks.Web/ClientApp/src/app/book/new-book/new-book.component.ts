import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ReactiveFormsModule, FormsModule, FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Category } from '../../models/category.model';
import { CategoryArea } from '../../models/category-area.model';
import { Book } from '../../models/book.model';
import { CategoryService } from '../../services/category.service';
import { CategoryAreaService } from '../../services/category-area.service';
import { BookService } from '../../services/book.service';

@Component({
  selector: 'app-new-book',
  templateUrl: './new-book.component.html',
  styleUrls: ['./new-book.component.css']
})
export class NewBookComponent implements OnInit {
  newBookForm: FormGroup;
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
    private categoryAreaService: CategoryAreaService,
    private categoryService: CategoryService,
    private bookService: BookService,
    private toastr: ToastrService,
    fb: FormBuilder) {
    this.newBookForm = fb.group({
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
    this.name = this.newBookForm.controls['name'] as FormControl;
    this.area = this.newBookForm.controls['area'] as FormControl;
    this.category = this.newBookForm.controls['category'] as FormControl;
    this.year = this.newBookForm.controls['year'] as FormControl;
    this.authors = this.newBookForm.controls['authors'] as FormControl;
    this.category = this.newBookForm.controls['category'] as FormControl;
    this.imageUri = this.newBookForm.controls['imageUri'] as FormControl;
    this.amazonUri = this.newBookForm.controls['amazonUri'] as FormControl;
    this.downloadUri = this.newBookForm.controls['downloadUri'] as FormControl;
    this.reflection = this.newBookForm.controls['reflection'] as FormControl;
  }

  ngOnInit() {
    this.categoryAreaService.getAll()
      .subscribe(areas => {
        this.categoryAreas = areas as CategoryArea[];
        
        this.categoryService.getAll()
          .subscribe(categories => {
            this.allCategories = categories as Category[];

            if (areas.length > 0) {
              this.selectedAreaId = areas[0].id;
              this.populateCategories();
            }
          });
      });
  }

  populateCategories = () => {
    let areaId = this.selectedAreaId;
    this.categories.length = 0;
    for (let c of this.allCategories) {
      if (c.areaId == areaId) {
        this.categories.push(c);
      }
    }
    this.selectedCategoryId = undefined;
    if (areaId > 0 && this.categories.length > 0) {
      this.selectedCategoryId = this.categories[0].id;
    }
  }

  categoryAreaChange = () => {
    this.populateCategories();
  }
  
  goBack = () => {
    this.location.back();
  }

  onSubmit = () => {
    if (this.newBookForm.valid) {
      if (this.selectedAreaId == undefined || this.selectedAreaId == 0) {
        this.errorMessage = "Area* is required";
        return;
      }

      if (this.selectedCategoryId == undefined || this.selectedCategoryId == 0) {
        this.errorMessage = "Category* is required";
        return;
      }

      var newBook = new Book();
      newBook.name = this.newBookForm.value["name"];
      newBook.areaId = this.newBookForm.value["area"];
      newBook.areaName = this.getAreaName(newBook.areaId);
      newBook.categoryId = this.newBookForm.value["category"];
      newBook.categoryName = this.getCategoryName(newBook.categoryId);            
      newBook.publishYear = this.newBookForm.value["year"];
      newBook.authors = this.newBookForm.value["authors"];
      newBook.imageUri = this.newBookForm.value["imageUri"];
      newBook.amazonUri = this.newBookForm.value["amazonUri"];
      newBook.contentUri = this.newBookForm.value["downloadUri"];
      newBook.reflection = this.newBookForm.value["reflection"];      

      this.bookService.create(newBook).subscribe(
        (data) => {
          this.errorMessage = '';
          this.toastr.success('Book created successfully')
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
