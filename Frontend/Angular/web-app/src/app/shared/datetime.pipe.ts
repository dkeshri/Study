import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { Format } from './models/app-constants';

@Pipe({
  name: 'datetime'
})
export class DatetimePipe implements PipeTransform {

  transform(value: Date | string, format: string = Format.DATETIME): string|null {
    value = new Date(value);
    return new DatePipe('en-US').transform(value, format);
  }

}
