import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';
import { AppConstants } from 'src/app/shared/models/app-constants';

@Component({
  selector: 'app-redirect-to-landing-page',
  templateUrl: './redirect-to-landing-page.component.html',
  styleUrls: ['./redirect-to-landing-page.component.css']
})
export class RedirectToLandingPageComponent implements OnInit {

  constructor(private userService:UserService,
    private router: Router,) { }

  ngOnInit(): void {
    this.userService.isSuperAdmin().subscribe((isSuperAdmin)=>{
      if(isSuperAdmin){
        this.router.navigate(['admin/']);
      }else{
        this.router.navigate(['pages/']);
      }
    },(error:HttpErrorResponse)=>{
        if(error.status == 0){
          this.router.navigate([AppConstants.LOGIN_URL]);
        }
    })
  }

}
