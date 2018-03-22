import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule, MatCheckboxModule } from '@angular/material';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { NgBootstrapFormValidationModule } from 'ng-bootstrap-form-validation';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { DialogService } from './services/dialog.service';
import { HomeComponent } from './home/home.component';
import { BookComponent } from './book/book.component';
import { CategoryComponent } from './category/category.component';
import { CategoryAreaComponent } from './category-area/category-area.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { NewCategoryAreaComponent } from './category-area/new-category-area/new-category-area.component';
import { EditCategoryAreaComponent } from './category-area/edit-category-area/edit-category-area.component';

import { ErrorInterceptor} from './interceptors/error-interceptor';
import { CategoryAreaService } from './services/category-area.service';
import { CategoryService } from './services/category.service';
import { BookService } from './services/book.service';
import { NewCategoryComponent } from './category/new-category/new-category.component';
import { EditCategoryComponent } from './category/edit-category/edit-category.component';
import { NewBookComponent } from './book/new-book/new-book.component';
import { EditBookComponent } from './book/edit-book/edit-book.component';
import { PageMenuComponent } from './shared/page-menu/page-menu.component';
import { AlertErrorComponent } from './shared/alert-error/alert-error.component';
import { FormBarComponent } from './shared/form-bar/form-bar.component';

const routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'category-area', component: CategoryAreaComponent },
  { path: 'category-area/new', component: NewCategoryAreaComponent },
  { path: 'category-area/:id', component: EditCategoryAreaComponent },
  { path: 'category', component: CategoryComponent },
  { path: 'category/new', component: NewCategoryComponent },
  { path: 'category/:id', component: EditCategoryComponent },
  { path: 'book', component: BookComponent },
  { path: 'book/new', component: NewBookComponent },
  { path: 'book/:id', component: EditBookComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    BookComponent,
    CategoryComponent,
    CategoryAreaComponent,
    ConfirmDialogComponent,
    NewCategoryAreaComponent,
    EditCategoryAreaComponent,
    NewCategoryComponent,
    EditCategoryComponent,
    NewBookComponent,
    EditBookComponent,
    PageMenuComponent,
    AlertErrorComponent,
    FormBarComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgBootstrapFormValidationModule.forRoot(),
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatDialogModule,
    MatSelectModule,
    NoopAnimationsModule,    
    RouterModule.forRoot(routes),
    CommonModule,
    BrowserAnimationsModule, // toastr needs animations module
    ToastrModule.forRoot({
      timeOut: 2000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    }), 
  ],
  providers: [
    DialogService,
    CategoryAreaService,
    CategoryService,
    BookService,
    { provide: 'BASE_URL', useFactory: getBaseUrl },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }  
  ],
  entryComponents: [
    ConfirmDialogComponent
  ],
  bootstrap: [AppComponent]
})

export class AppModule {
}

export function getBaseUrl() {
  var origin = window.location.origin;
  var base = document.getElementsByTagName('base')[0].href;  
  return origin;
}
