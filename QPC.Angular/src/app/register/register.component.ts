import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Services/auth.service';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private service: AuthService, private router:Router) { }

  ngOnInit() {
  }

  
  OnSubmit(email, password, confirmPassword){
    var resource ={
      email:email,
      password: password,
      confirmPassword: confirmPassword
    };
    console.log(resource);
    this.service.registerUser(resource)
      .subscribe(result => {
        this.service.userAuthentication(resource.email, resource.password)
          .subscribe((token:any) =>{
            localStorage.setItem('userToken', token.access_token);
            this.router.navigate(['/controls']);
          });
      });
  }

}
