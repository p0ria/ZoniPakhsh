import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Urls} from '../common/Config';
import {share} from 'rxjs/operators';
import {Observable} from 'rxjs';
import {Product} from '../models/Product';
import {ProductsResult} from '../models/ProductsResult';
import {NewProduct} from '../models/NewProduct';
import {User} from '../models/User';
import {NewUser} from '../models/NewUser';

@Injectable()
export class UserService {

  constructor(private http: HttpClient) { }

  public getAllAdmins(): Observable<User[]>{
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    return this.http.get<User[]>(Urls.Admins, {headers: headers});
  }

  public addUser(user: NewUser): Observable<User>{
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    let body = JSON.stringify(user);
    return this.http.post<User>(Urls.Admins, body, {headers});
  }

  public editUser(user: NewUser): Observable<User>{
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    let body = JSON.stringify(user);
    return this.http.put<User>(Urls.Admins, body, {headers});
  }

  public deleteUser(userId: number): Observable<boolean>{
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    return this.http.delete<boolean>(Urls.Admins + `/${userId}`, {headers});
  }

}
