import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Chat } from '../models/chat.model';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(private api: HttpClient) { }

  getById(id: string){
    return this.api.get<Chat>('/chats/' + id);
  }

  getAll(userId: string){
    return this.api.get<Chat[]>('/chats/users/' + userId);
  }

  create(firstUserId: string, secondUserId: string){
    return this.api.post<Chat>('/chats/', {firstUserId, secondUserId});
  }

  delete(id: string){
    return this.api.delete<Chat>('/chats/' + id);
  }
}
