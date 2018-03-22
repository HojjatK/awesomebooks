import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatTableDataSource, MatSort } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { Router } from "@angular/router";
import { ToastrService } from 'ngx-toastr';
import { CategoryArea } from '../models/category-area.model'
import { DialogService } from '../services/dialog.service';
import { CategoryAreaService } from '../services/category-area.service';

@Component({
  selector: 'app-category-area',
  templateUrl: './category-area.component.html',
  styleUrls: ['./category-area.component.css']
})
export class CategoryAreaComponent implements OnInit {
  displayedColumns = ['select', 'id', 'name', 'description'];
  dataSource = new MatTableDataSource([]);
  selection = new SelectionModel<CategoryArea>(false, []);
  hasSelection: boolean = false;  
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  categoryAreaFile: File;  
  errorsCount: number = 0;
  errorMessages: string[] = [];

  constructor(private router: Router,
    private dialogService: DialogService,
    private toastService : ToastrService,
    private categoryAreaService: CategoryAreaService) {}

  ngOnInit() {
    this.loadCategoryAreas();
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

  loadCategoryAreas = () => {
    this.categoryAreaService.getAll().subscribe(
      data => {
        this.dataSource.data = data;
        console.log(`category areas loaded successfully.`);
      });
  }

  newCategoryArea = () => {
    this.router.navigate(['category-area/new']);
  }

  editCategoryArea = () => {
    var selected = this.getSelected();
    if (selected != null) {
      var id = selected.id;
      this.router.navigate([`category-area/${id}`]);
    }
  }

  deleteCategoryArea = () => {
    var selected = this.getSelected();
    if (selected != null) {
      var id = selected.id;

      this.dialogService
        .confirm('Confirm Area Delete', `Are you sure you want to delete '${selected.name}' Area?`)
        .subscribe(confirmed => {
          if (confirmed) {
            this.categoryAreaService.delete(id).subscribe(
              (data) => {                
                console.log(`${selected.name} category area deleted successfully.`);
                this.selection.clear();
                this.hasSelection = false;
                this.toastService.success('Area deleted successfuly');
                this.loadCategoryAreas();
              });        
          }
        });
    }
  }

  getSelected = (): CategoryArea => {
    var selected: CategoryArea = this.selection.selected.length > 0
      ? this.selection.selected[0]
      : null;
    return selected;
  }

  onSubmitUpload = (fileToUpload: File) => {
    if (fileToUpload == undefined) {
      return;
    }

    this.categoryAreaService.upload(fileToUpload)
      .subscribe((data : any) => {
        this.toastService.success('Areas uploaded successfully');
        var errors = data.errorMessages as string[];
        if (data.errorsCount > 0) {         
          this.errorsCount = data.errorsCount;
          this.errorMessages = errors;
        }
        else {
          this.errorMessages.splice(0, this.errorMessages.length);
        }
        this.loadCategoryAreas();        
      });
  }
}
