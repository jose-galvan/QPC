import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import 'rxjs/add/operator/map';

@Injectable()
export class AuthService{

  rootUrl ='http://localhost:49529';

  constructor(private http: HttpClient) { }


  userAuthentication(userName, password) {
    var body = 'grant_type=password&username='+ userName +'&password=' + password;
    var reqHeader = new HttpHeaders({ 'Content-Type': 'application/json','Accept':'application/json' });
    return this.http.post('http://localhost:49529/token', body, { headers: reqHeader });
  }

  registerUser(resource: any){
    return this.http.post(this.rootUrl+'/api/account/register', resource);
  }

  GetUserName(){
    return  this.http.get(this.rootUrl+'/api/account/username');
  }

  IsAuthenticated(){
    if (localStorage.getItem('userToken') != null)
      return true;
    return false;
  }

  LogOut(){
    localStorage.removeItem('userToken');
    localStorage.clear();
  }

}
