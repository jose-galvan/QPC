import { Router } from '@angular/router';
import { AuthService } from './../Services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent  implements OnInit  {
  
  username: string;
  
  constructor(private service: AuthService, private router: Router) { }
 
  ngOnInit(): void {
    this.setUserName();
  }
  
  IsAuthenticated(){
    return this.service.IsAuthenticated();
  }
  
  LogOut(){
    this.service.LogOut();
    this.router.navigateByUrl('/login');
  }

  setUserName(){
    if(this.IsAuthenticated())
    {
      this.service.GetUserName()
        .subscribe(result =>{
          this.username = result.toString();
        });
    }
  }

}
