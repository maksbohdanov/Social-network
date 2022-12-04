import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { RegistrationModel } from '../models/registration.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private api: HttpClient,private roter: Router) { }

  register(model : RegistrationModel){
    return this.api.post<User>('/users/register', model);
  }
}
