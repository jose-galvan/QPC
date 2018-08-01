import { HttpInterceptor, HttpRequest, HttpHandler, HttpUserEvent, HttpEvent } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/do';
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(private router: Router) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${localStorage.getItem('userToken')}`
            }
          });
          return next.handle(request);
    }
}