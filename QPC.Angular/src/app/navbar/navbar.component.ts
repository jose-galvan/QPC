import { Router } from '@angular/router';
import { AuthService } from './../Services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {

  constructor(private authService: AuthService, private router: Router) { }

  IsAuthenticated(){
    return this.authService.IsAuthenticated();
  }

  LogOut(){
    this.authService.LogOut();
    this.router.navigateByUrl('/login');
  }

}
