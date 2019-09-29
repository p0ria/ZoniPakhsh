import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Urls} from '../common/Config';
import {LoginResponse} from '../models/LoginResponse';


const TOKEN_KEY = 'zoin_token';

@Injectable()
export class LoginService {
  private _token: string;
  get token() : string{
    return this._token;
  }
  set token(value : string){
    this._token = value;
    localStorage.setItem(TOKEN_KEY, value);
  }

  get isAuthenticated(): boolean{
    return this.token != null;
  }


  constructor(private http: HttpClient) {
    this.loadToken();
  }

  public login(username: string, password: string): Observable<LoginResponse> {
    let headers = new HttpHeaders({
      'Content-Type': 'application/x-www-form-urlencoded',
      'No-Auth': 'True'});
    let body = `username=${username}&password=${password}&grant_type=password`;
    return this.http.post<LoginResponse>(Urls.Token, body, {headers: headers});
  }

  public logout(){
    this._token = null;
    localStorage.removeItem(TOKEN_KEY);
  }

  private loadToken(){
    this._token = localStorage.getItem(TOKEN_KEY);
  }
}

