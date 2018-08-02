import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Services/auth.service';
import { Router } from '../../../node_modules/@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  userClaims: any;
  
  constructor(private router: Router, private authService: AuthService) { }

  ngOnInit() {
    this.authService.GetUserName().subscribe((data: any) => {
      this.userClaims = data;
 
    });
  }
 
  Logout() {
    localStorage.removeItem('userToken');
    this.router.navigate(['/login']);
  }

}
