import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Chat } from 'src/app/models/chat.model';
import { Message } from 'src/app/models/message.model';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { ChatService } from 'src/app/services/chat.service';
import { MessageService } from 'src/app/services/message.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit{
  currentUserId!: string;  
  chatid!: string;
  messages!: Message[];
  messageContent!: string;

  constructor(private chatService: ChatService,
    private messageService: MessageService,
    private authService: AuthService,    
    private userService: UserService,
    private route: ActivatedRoute){} 

  ngOnInit(): void {
    this.currentUserId = this.authService.getUserId();
    this.route.params.subscribe(params =>  this.chatid = params['id']);
    this.getMessages();
  }

  getMessages(){
    this.messageService.getAll(this.chatid)
      .subscribe(messages => {
        this.messages = messages;
      });
  }

  sendMessage(){
    if(this.messageContent && this.messageContent.trim().length > 0){
      this.messageService.send(this.messageContent, this.currentUserId, this.chatid)
        .subscribe(() => window.location.reload());
    }
  }

}
