import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatTableDataSource, MatSort } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { Router } from "@angular/router";
import { ToastrService } from 'ngx-toastr';
import { Book } from '../models/book.model'
import { DialogService } from '../services/dialog.service';
import { BookService } from '../services/book.service';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css']
})
export class BookComponent implements OnInit {
  displayedColumns = ['select', 'id', 'image', 'name', 'areaName', 'categoryName', 'authors', 'publishYear'];
  dataSource = new MatTableDataSource([]);
  selection = new SelectionModel<Book>(false, []);
  hasSelection: boolean = false;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  bookFile: File;
  errorsCount: number = 0;
  errorMessages: string[] = [];

  constructor(private router: Router,
    private dialogService: DialogService,
    private toastService: ToastrService,
    private bookService: BookService) {}

  ngOnInit() {
    this.loadBooks();
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

  loadBooks = () => {
    this.bookService.getAll().subscribe(
      data => {
        this.dataSource.data = data;
        console.log(`books loaded successfully.`);
      });
  }

  newBook = () => {
    this.router.navigate(['book/new']);
  }

  editBook = () => {
    var selected = this.getSelected();
    if (selected != null) {
      var id = selected.id;
      this.router.navigate([`book/${id}`]);
    }
  }

  deleteBook = () => {
    var selected = this.getSelected();
    if (selected != null) {
      var id = selected.id;

      this.dialogService
        .confirm('Confirm Book Delete', `Are you sure you want to delete '${selected.name}' Book?`)
        .subscribe(confirmed => {
          if (confirmed) {
            this.bookService.delete(id).subscribe(
              (data) => {
                console.log(`${selected.name} book deleted successfully.`);
                this.selection.clear();
                this.hasSelection = false;
                this.toastService.success('Book deleted successfuly');
                this.loadBooks();
              });
          }
        });
    }
  }

  getSelected = (): Book => {
    var selected: Book = this.selection.selected.length > 0
      ? this.selection.selected[0]
      : null;
    return selected;
  }

  onSubmitUpload = (fileToUpload: File) => {
    if (fileToUpload == undefined) {
      return;
    }

    this.bookService.upload(fileToUpload)
      .subscribe((data: any) => {
        this.toastService.success('Books uploaded successfully');
        var errors = data.errorMessages as string[];
        if (data.errorsCount > 0) {
          this.errorsCount = data.errorsCount;
          this.errorMessages = errors;
        }
        else {
          this.errorMessages.splice(0, this.errorMessages.length);
        }
        this.loadBooks();
      });
  }
}
