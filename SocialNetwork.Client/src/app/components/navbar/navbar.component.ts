import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  constructor(
    private authService: AuthService,
    private router: Router){}

    isAdmin(){
      return this.authService.isAdmin;
    }

    isAuthenticated(){
      return this.authService.isLoggedIn;
    }

    logout(){
      this.authService.logout();
    }

    navigateToProfile(){
      this.router.navigate(['profile/' + this.authService.getUserId()]);
    }
    
}
