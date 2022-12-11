import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { tap } from "rxjs";
import { LoginModel } from "../models/login.model";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private jwtHelper = new JwtHelperService();

  constructor(private api: HttpClient,
    private roter: Router) {
      if (this.isLoggedIn && this.jwtHelper.isTokenExpired(this.token)) {
      this.logout();
      }
  }

  public login(email: string, password: string) {
    return this.api.post('/users/login', <LoginModel>{email, password}, {responseType: 'text'})
      .pipe(
        tap(this.setToken)
      );
  }

  public logout() {
    localStorage.removeItem('token');
  }

  public getUserId(){
    if(this.isLoggedIn){
      const nameIdentifier: string = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
      const decodedToken = this.jwtHelper.decodeToken(this.token);
      
      return decodedToken[nameIdentifier];
    }    
  }

  get isLoggedIn(): boolean {
    return this.token !== undefined;
  }

  get isAdmin(): boolean{
    if(!this.isLoggedIn){
      return false;
    }
    else{
      const roleClaims: string = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
      const decodedToken = this.jwtHelper.decodeToken(this.token);

      return decodedToken[roleClaims].includes("Admin");
    }
  }

  private setToken(token: string) {
    localStorage.setItem('token', token);
  }

  get token(): string | undefined {
    const token = localStorage.getItem('token');
    if (!token || this.jwtHelper.isTokenExpired(token)) {
      return;
    }

    return token;
  }
}
