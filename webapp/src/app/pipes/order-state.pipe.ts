import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'orderState'
})
export class OrderStatePipe implements PipeTransform {

  transform(value: number, args?: any): any {
    switch (value) {
      case 0: return 'سبد خرید';
      case 1: return 'ثبت شده';
      case 2: return 'ارسال شده';
      default: return 'تحویل داده شده';
    }
  }

}
