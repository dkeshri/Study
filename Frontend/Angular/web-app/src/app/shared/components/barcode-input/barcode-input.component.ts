import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-barcode-input',
  templateUrl: './barcode-input.component.html',
  styleUrls: ['./barcode-input.component.css']
})
export class BarcodeInputComponent implements OnInit {

  @Input()
  barcode: string = '';
  @Input()
  placeholder: string = '';
  @Output()
  EnterKeyPressed: EventEmitter<Event> = new EventEmitter<Event>();
  @Output()
  barcodeChange: EventEmitter<string> = new EventEmitter<string>();
  constructor() { }

  ngOnInit(): void {
  }
  onEnterPress(event:Event){
    this.barcodeChange.emit(this.barcode);
    this.EnterKeyPressed.emit(event);
  }
}
