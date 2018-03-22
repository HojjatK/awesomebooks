import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { Category } from '../models/category.model';

@Injectable()
export class CategoryService {
  baseUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getAll(): Observable<Category[]> {
    var url = `${this.baseUrl}/api/category`;
    return this.http.get<Category[]>(url);

  }

  get = (id: number): Observable<Category> => {
    var url = `${this.baseUrl}/api/category/${id}`;
    return this.http.get<Category>(url);
  }

  create = (category: Category) => {
    var url = `${this.baseUrl}/api/category`;
    return this.http.post(url, category);
  }

  update = (category: Category) => {
    var url = `${this.baseUrl}/api/category/${category.id}`;
    return this.http.put(url, category);
  }

  delete = (categoryId: number) => {
    var url = `${this.baseUrl}/api/category/${categoryId}`;
    return this.http.delete(url);
  }

  upload = (file: File) => {
    let formData = new FormData();
    formData.append("file", file, file.name);
    let body = formData;
    var url = `${this.baseUrl}/api/category/upload`;
    return this.http.post(url, body);
  }
}
