import {User} from './User';
import {OrderItem} from './OrderItem';

export class Order{
  Id?: number;
  State: number;
  User: User;
  Items: OrderItem[] = [];
  TotalPrice: number;
  TotalCount: number;
  ItemsAvailable: boolean;
  SubmitDatePersian: string;
  DeliverDate: string;
}
