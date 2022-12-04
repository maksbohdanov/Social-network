import {  Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RegistrationModel } from 'src/app/models/registration.model';
import { NotificationService } from 'src/app/services/notification.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {
  form: any = {
    firstName: null,
    lastName: null,
    city: null,
    birthDate: null,
    email: null,
    password: null
  };
  
  constructor(private userService: UserService,
    private notificationService: NotificationService,
    private router: Router) {
  }

  ngOnInit(): void {
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }
    const { firstName, lastName, city, birthDate, email, password } = this.form; 
    let model = {
      firstName: firstName,
      lastName: lastName,
      city: city,
      birthDate: birthDate,
      email: email,
      password: password
      }
    
    this.userService.register(model)
      .subscribe({
        next: () => {
          this.router.navigate(['login']);
        },
        error: err => {
          this.notificationService.notifyError(`Registration failed. ${err.error?.Message ?? ''}`);
        }
      });
  }
}
