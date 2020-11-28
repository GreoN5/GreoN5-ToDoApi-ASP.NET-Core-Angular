import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(private router: Router) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (localStorage.getItem('token') != null) {
            let request = req.clone({
                headers: req.headers.set('Authorization', 'Bearer ' + localStorage.getItem('token'))
            });

            return next.handle(request).pipe(
                tap(
                    success => { },
                    error => {
                        if (error.status == 401) {
                            localStorage.clear();
                            alert('You are not authorized in order to access this page!');
                            this.router.navigateByUrl("user/login");
                        }
                    }
                )
            )
        } else {
            return next.handle(req.clone());
        }
    }
}