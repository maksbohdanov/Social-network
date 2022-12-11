import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Message } from '../models/message.model';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private api: HttpClient) { }

  getAll(chatId: string){
    return this.api.get<Message[]>('/messages/chats/' + chatId);
  }

  send(text: string, authorId: string, chatId: string){
    return this.api.post('/messages', {text, authorId, chatId});
  }
}
