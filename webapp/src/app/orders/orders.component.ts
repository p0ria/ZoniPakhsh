import { Component, OnInit } from '@angular/core';
import {OrderService} from '../services/order.service';
import {Order} from '../models/Order';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  selected: Order;
  isDeleteModalOpen: boolean = false;

  constructor(public orderService: OrderService) { }

  ngOnInit() {
    this.orderService.getAllOrders();
  }

  onChangeState(order: Order, newState: number): void{
    this.orderService.changeOrderState(order, newState);
  }

  onDelete(order: Order){
    this.selected = order;
    this.showDeleteModal();
  }

  onDeleteCancel(){
    this.hideDeleteModal();
  }

  onDeleteOK(){
    this.orderService.deleteOrder(this.selected.Id);
    this.hideDeleteModal();
  }

  showDeleteModal(){
    this.isDeleteModalOpen = true;
  }

  hideDeleteModal(){
    this.isDeleteModalOpen = false;
  }



}
