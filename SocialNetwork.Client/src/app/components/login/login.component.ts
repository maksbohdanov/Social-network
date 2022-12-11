import {  Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  form: any = {
    email: null,
    password: null
  };


  constructor(private auth: AuthService,
    private notificationService: NotificationService,
    private router: Router) {
  }

  ngOnInit(): void {
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }
    const { email, password } = this.form;

    this.auth.login(email, password)
      .subscribe({
        next: () => {
          this.router.navigate([`profile/${this.auth.getUserId()}`]);
        },
        error: err => {
          this.notificationService.notifyError(`Authentication failed. ${err.error?.message ?? ''}`);
        }
      });
  }
}
