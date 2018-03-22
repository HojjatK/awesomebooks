import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatTableDataSource, MatSort } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { Router } from "@angular/router";
import { ToastrService } from 'ngx-toastr';
import { Category } from '../models/category.model'
import { DialogService } from '../services/dialog.service';
import { CategoryService } from '../services/category.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {
  displayedColumns = ['select', 'id', 'areaName', 'name', 'description'];
  dataSource = new MatTableDataSource([]);
  selection = new SelectionModel<Category>(false, []);
  hasSelection: boolean = false;  
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  categoryFile: File;  
  errorsCount: number = 0;
  errorMessages: string[] = [];

  constructor(private router: Router,
    private dialogService: DialogService,
    private toastService: ToastrService,
    private categoryService: CategoryService) {}

  ngOnInit() {
    this.loadCategories();
  }

  /**
   * Set the paginator after the view init since this component will
   * be able to query its view for the initialized paginator.
   */
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  applyFilter = (filterValue: string) => {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }

  toggleSelection = (row: any) => {
    this.selection.toggle(row)
    this.hasSelection = (this.selection.selected.length > 0);
  }

  loadCategories = () => {
    this.categoryService.getAll().subscribe(
      data => {
        this.dataSource.data = data;
        console.log(`categories loaded successfully.`);
      });
  }

  newCategory = () => {
    this.router.navigate(['category/new']);
  }

  editCategory = () => {
    var selected = this.getSelected();
    if (selected != null) {
      var id = selected.id;
      this.router.navigate([`category/${id}`]);
    }
  }

  deleteCategory = () => {
    var selected = this.getSelected();
    if (selected != null) {
      var id = selected.id;

      this.dialogService
        .confirm('Confirm Category Delete', `Are you sure you want to delete '${selected.name}' Category?`)
        .subscribe(confirmed => {
          if (confirmed) {
            this.categoryService.delete(id).subscribe(
              (data) => {
                console.log(`${selected.name} category deleted successfully.`);
                this.selection.clear();
                this.hasSelection = false;
                this.toastService.success('Category deleted successfuly');
                this.loadCategories();
              });
          }
        });
    }
  }

  getSelected = (): Category => {
    var selected: Category = this.selection.selected.length > 0
      ? this.selection.selected[0]
      : null;
    return selected;
  }
  
  onSubmitUpload = (fileToUpload: File) => {
    if (fileToUpload == undefined) {
      return;
    }

    this.categoryService.upload(fileToUpload)
      .subscribe((data: any) => {
        this.toastService.success('Category uploaded successfully');
        var errors = data.errorMessages as string[];
        if (data.errorsCount > 0) {
          this.errorsCount = data.errorsCount;
          this.errorMessages = errors;
        }
        else {
          this.errorMessages.splice(0, this.errorMessages.length);
        }
        this.loadCategories();
      });
  }
}
