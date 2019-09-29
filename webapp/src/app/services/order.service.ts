import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Order} from '../models/Order';
import {Urls} from '../common/Config';
import { Router} from '@angular/router';
import {Observable} from 'rxjs';

@Injectable()
export class OrderService {
  public orders: Order[] = [];

  constructor(private http: HttpClient,
              private router: Router) { }

  public getAllOrders(): void{
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    let sub = this.http.get<Order[]>(Urls.Orders, {headers}).subscribe(
      (orders: Order[]) => {
        this.orders = orders;
      },
      error => {
        console.log(error);
        if (error.status === 401)
          this.router.navigate(['/login']);
        sub.unsubscribe();
      }
    )
  }

  public changeOrderState(order: Order, newState: number): void{
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    let url = Urls.Orders + `/${order.Id}?state=${newState}`;
    let sub = this.http.put<Order>(url, null, {headers}).subscribe(
      (resultOrder: Order) => {
        order.State = resultOrder.State;
        sub.unsubscribe();
      },
      error => {
        console.log(error);
        if (error.status === 401)
          this.router.navigate(['/login']);
        sub.unsubscribe();
      }
    )
  }

  public deleteOrder(orderId: number): void{
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'});
    let url = Urls.Orders + `/${orderId}`;
    let sub = this.http.delete<boolean>(url, {headers}).subscribe(
      (succeed: boolean) => {
        if(succeed){
          let index = this.orders.findIndex(o => o.Id == orderId);
          if(index > -1) this.orders.splice(index, 1);
        }
        sub.unsubscribe();
      },
      error => {
        console.log(error);
        if (error.status === 401)
          this.router.navigate(['/login']);
        sub.unsubscribe();
      }
    )
  }
}
