import {Component, EventEmitter, OnInit} from '@angular/core';
import {ClrDatagridFilterInterface} from '@clr/angular';
import {Order} from '../models/Order';

@Component({
  selector: 'order-status-filter',
  templateUrl: './order-status-filter.component.html',
  styleUrls: ['./order-status-filter.component.scss']
})
export class OrderStatusFilterComponent implements OnInit, ClrDatagridFilterInterface<Order> {
  isSubmitted: boolean = true;
  isSent: boolean = true;
  isDelivered: boolean = true;

  constructor() { }

  ngOnInit() {
  }

  changes: EventEmitter<any> = new EventEmitter<any>(false);
  readonly state: any;

  accepts(item: Order): boolean {
    if(this.isSubmitted) {
      if(item.State == 1) return true;
    }
    if(this.isSent){
      if(item.State == 2) return true;
    }
    if(this.isDelivered){
      if(item.State == 3) return true;
    }
    return false;
  }

  equals(other: ClrDatagridFilterInterface<Order, any>): boolean {
    return false;
  }

  isActive(): boolean {
    return !this.isSubmitted || !this.isSent || !this.isDelivered;
  }

  setIsSubmitted(value: any){
    this.isSubmitted = value.target.checked;
    this.changes.emit(true);
  }

  setIsSent(value: any){
    this.isSent = value.target.checked;
    this.changes.emit(true);
  }

  setIsDelivered(value: any){
    this.isDelivered = value.target.checked;
    this.changes.emit(true);
  }


}
