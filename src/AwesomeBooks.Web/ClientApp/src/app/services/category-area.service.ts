import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { CategoryArea } from '../models/category-area.model';

@Injectable()
export class CategoryAreaService {
  baseUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;    
  }

  getAll(): Observable<CategoryArea[]> {
    var url = `${this.baseUrl}/api/category-area`;
    return this.http.get<CategoryArea[]>(url);

  }

  get = (id: number): Observable<CategoryArea> => {
    var url = `${this.baseUrl}/api/category-area/${id}`;
    return this.http.get<CategoryArea>(url);
  }

  create = (area: CategoryArea) => {
    var url = `${this.baseUrl}/api/category-area`;
    return this.http.post(url, area);
  }

  update = (area: CategoryArea) => {
    var url = `${this.baseUrl}/api/category-area/${area.id}`;
    return this.http.put(url, area);
  }

  delete = (areaId: number) => {
    var url = `${this.baseUrl}/api/category-area/${areaId}`;
    return this.http.delete(url);
  }

  upload = (file: File) => {
    let formData = new FormData();
    formData.append("file", file, file.name);
    let body = formData;    
    var url = `${this.baseUrl}/api/category-area/upload`;
    return this.http.post(url, body);
  }
}
