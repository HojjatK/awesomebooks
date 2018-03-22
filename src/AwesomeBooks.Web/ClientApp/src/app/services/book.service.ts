import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { Book } from '../models/book.model';

@Injectable()
export class BookService {
  baseUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getAll(): Observable<Book[]> {
    var url = `${this.baseUrl}/api/book`;
    return this.http.get<Book[]>(url);
  }

  get = (id: number): Observable<Book> => {
    var url = `${this.baseUrl}/api/book/${id}`;
    return this.http.get<Book>(url);
  }

  create = (book: Book) => {
    var url = `${this.baseUrl}/api/book`;
    return this.http.post(url, book);
  }

  update = (book: Book) => {
    var url = `${this.baseUrl}/api/book/${book.id}`;
    return this.http.put(url, book);
  }

  delete = (bookId: number) => {
    var url = `${this.baseUrl}/api/book/${bookId}`;
    return this.http.delete(url);
  }

  upload = (file: File) => {
    let formData = new FormData();
    formData.append("file", file, file.name);
    let body = formData;
    var url = `${this.baseUrl}/api/book/upload`;
    return this.http.post(url, body);
  }
}
