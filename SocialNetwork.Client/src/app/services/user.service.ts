import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Friendship } from '../models/friendship.model';
import { RegistrationModel } from '../models/registration.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private api: HttpClient,private roter: Router) { }

  getById(id: string){
    return this.api.get<User>('/users/' + id);
  }

  getAll(){
    return this.api.get<User[]>('/users');
  }

  fingByFilter(filter: string){
    return this.api.get<User[]>(`/users/filter/${filter}` );
  }

  register(model : RegistrationModel){
    return this.api.post<User>('/users/register', model);
  }

  edit(model: User){
    return this.api.put<User>('/users/update', model)
  }

  getFriends(id: string){
    return this.api.get<User[]>(`/users/${id}/friends`)
  }

  addToFriends(userId: string, friendId: string){
    return this.api.put(`/users/${friendId}/${userId}`, null);
  }

  getRequests(id: string){
    return this.api.get<Friendship[]>(`/users/${id}/requests`)
  }

  approveFriendship(friendshipId: string, answer: boolean){
    return this.api.put(`/users/friendship/${friendshipId}/${answer}`, null);
  }

  delete(id: string){
    return this.api.delete<boolean>('/users/' + id);
  }
}
