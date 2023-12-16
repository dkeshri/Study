import { Component, OnInit, } from '@angular/core';
import { Router } from '@angular/router';
import { faUserLock } from '@fortawesome/free-solid-svg-icons';
import { AuthenticationService } from 'src/app/core/services/auth/authentication.service';
import { UserService } from 'src/app/services/user.service';
interface UserCrediential{
  userName:string,
  password:string
}
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  faUserLock = faUserLock;
  size = '20px';
  red = 'pink';
  shopId:number = 0;
  userName:string = '';
  password:string = '';
  //
  faUserLockStyles = {
    color: 'rgb(63 81 181)'
   };
  constructor(
    private authenticationService:AuthenticationService,
    private router:Router,
    private userService:UserService
    ) {
  }

  ngOnInit(): void {
  }

  onSubmit(){
    let userCrediential:UserCrediential = {} as UserCrediential;
    userCrediential.userName = this.userName;
    userCrediential.password = this.password;
    this.authenticationService.login(userCrediential)
    .subscribe((response)=>{
      if(response){
        this.router.navigate(['']);
      }
    });
  }
}
