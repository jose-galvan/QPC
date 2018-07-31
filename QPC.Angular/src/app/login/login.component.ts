import { Component } from '@angular/core';
import {Router } from '../../../node_modules/@angular/router';
import { AuthService } from '../Services/auth.service';
import { HttpErrorResponse } from '../../../node_modules/@angular/common/http';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent{
  isLoginError: boolean; 

  constructor(
    private router: Router, 
    private authService: AuthService) { }

    OnSubmit(userName,password){

      this.authService.userAuthentication(userName, password)
          .subscribe( (result: any) => {
            localStorage.setItem('userToken', result.access_token);
            this.router.navigate(['/controls']);
          });



   }
}
