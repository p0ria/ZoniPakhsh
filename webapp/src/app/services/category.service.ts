import { Injectable } from '@angular/core';
import {Category} from '../models/Category';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Urls} from '../common/Config';
import {Observable} from 'rxjs';
import {NewCategory} from '../models/NewCategory';
import {Router} from '@angular/router';
import {NewProduct} from '../models/NewProduct';
import {Product} from '../models/Product';

@Injectable()
export class CategoryService {
  public categories: Category[] = [];

  constructor(private http: HttpClient,
              private router: Router) {}

  getCategories(){
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    let getSub = this.http.get<Category[]>(Urls.Categories, {headers: headers})
      .subscribe(
        (categories: Category[]) =>{
          this.categories = categories;
        },
        error => {
          console.log(error);
          if (error.status === 401)
            this.router.navigate(['/login']);
          getSub.unsubscribe();
        });
  }

  addCategory(category: NewCategory){
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    let body = JSON.stringify(category);
    let addSub = this.http.post<Category>(Urls.Categories, body, {headers})
      .subscribe(
        (category: Category) => {
          this.categories.push(category);
          addSub.unsubscribe();
        }, error => {
          console.log(error);
          if (error.status === 401)
            this.router.navigate(['/login']);
          addSub.unsubscribe();
        }
      );
  }

  editCategory(category: NewCategory){
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    let body = JSON.stringify(category);
    let sub = this.http.put<Product>(Urls.Categories, body, {headers})
      .subscribe((category: Category) => {
        let toBeUpdated = this.categories.find(c => c.Id == category.Id);
        if(toBeUpdated) {
          toBeUpdated.Name = category.Name;
          toBeUpdated.ImageUrl = category.ImageUrl;
        }
        sub.unsubscribe();
      }, error => {
        console.log(error);
        if (error.status === 401)
          this.router.navigate(['/login']);
        sub.unsubscribe();
      });
  }

  deleteCategory(categoryId: number){
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    let sub = this.http.delete<boolean>(Urls.Categories + `/${categoryId}`, {headers}).subscribe(
      (deleted: boolean) => {
        if(deleted){
          let index = this.categories.findIndex(c => c.Id == categoryId);
          if(index > -1) this.categories.splice(index, 1);
        }
        sub.unsubscribe();
      },
      error => {
        console.log(error);
        if (error.status === 401)
          this.router.navigate(['/login']);
        sub.unsubscribe();
      }
    );;
  }
}
