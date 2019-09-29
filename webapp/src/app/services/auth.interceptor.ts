import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import {LoginService} from './login.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private router: Router,
              private loginService: LoginService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.headers.get('No-Auth') == "True")
      return next.handle(req.clone());

    if (this.loginService.isAuthenticated) {
      const clonedreq = req.clone({
        headers: req.headers.set("Authorization", "Bearer " + this.loginService.token)
      });
      return next.handle(clonedreq);
    }
    else {
      this.loginService.logout();
      this.router.navigate(['/login']);
    }
  }
}
