import { Pipe, PipeTransform } from '@angular/core';
import {Urls} from '../common/Config';

@Pipe({
  name: 'baseAddress'
})
export class BaseAddressPipe implements PipeTransform {

  transform(value: string, args?: any): any {
    return Urls.Base + value;
  }

}
