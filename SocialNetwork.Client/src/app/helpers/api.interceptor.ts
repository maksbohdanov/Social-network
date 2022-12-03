import {Injectable} from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpStatusCode, HttpErrorResponse
} from '@angular/common/http';
import {catchError, EMPTY, Observable, throwError} from 'rxjs';
import {AuthService} from "../services/auth.service";
import {environment as env} from "../../environments/environment";
import {Router} from "@angular/router";
import {NotificationService} from "../services/notification.service";

@Injectable()
export class ApiInterceptor implements HttpInterceptor {

  constructor(private auth: AuthService,
              private notificationService: NotificationService,
              private router: Router) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    request = request.clone({url: `${env.baseApiUrl}${request.url}`});

    if (this.auth.isLoggedIn) {
      const token = this.auth.token ?? "";

      request = request.clone({
        headers: request.headers.set('Authorization', `Bearer ${token}`)
      });
    }
    
    return next.handle(request).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err.status === HttpStatusCode.Unauthorized) {
          this.router.navigate(['/login']);
          this.notificationService.notifyError("Please login.");
          return EMPTY;
        }
        console.log(err.message);
        return throwError(() => new Error(err.message));
      })
    );
  }
}
