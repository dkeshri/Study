import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'activeInactive'
})
export class ActiveInactivePipe implements PipeTransform {

  transform(value: boolean, ...args: unknown[]): string {
    let data:string = 'Common.InActive';
    if(value===true){
      data = 'Common.Active';
    }
    return data;
  }

}
