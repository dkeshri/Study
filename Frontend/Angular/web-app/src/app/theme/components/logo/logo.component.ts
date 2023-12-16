import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppConstants } from 'src/app/shared/models/app-constants';

@Component({
  selector: 'app-logo',
  templateUrl: './logo.component.html',
  styleUrls: ['./logo.component.css']
})
export class LogoComponent implements OnInit {

  @Input()
  shopName!: string;
  constructor(private router: Router) { }

  ngOnInit(): void {
  }
  onLogoClick(){
    //this.router.navigate([AppConstants.HOME_URL]);
  }
}
