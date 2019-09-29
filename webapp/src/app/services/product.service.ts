import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Urls} from '../common/Config';
import {Observable} from 'rxjs';
import {ProductsResult} from '../models/ProductsResult';
import {Product} from '../models/Product';
import {NewProduct} from '../models/NewProduct';

@Injectable()
export class ProductService {

  constructor(private http: HttpClient) { }

  public getAllProducts(): Observable<Product[]>{
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    return this.http.get<Product[]>(Urls.Products, {headers: headers});
  }

  public getCategoryProducts(categoryId?: number): Observable<ProductsResult>{
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    let url = Urls.Products + '?page=1' +
      (categoryId ? `&categoryId=${categoryId}` : '');
    return this.http.get<ProductsResult>(url,{headers: headers});
  }

  public addProduct(product: NewProduct): Observable<Product>{
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    let body = JSON.stringify(product);
    return this.http.post<Product>(Urls.Products, body, {headers});
  }

  public editProduct(product: NewProduct): Observable<Product>{
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    let body = JSON.stringify(product);
    return this.http.put<Product>(Urls.Products, body, {headers});
  }

  public deleteProduct(productId: number): Observable<boolean>{
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    return this.http.delete<boolean>(Urls.Products + `/${productId}`, {headers});
  }
}
