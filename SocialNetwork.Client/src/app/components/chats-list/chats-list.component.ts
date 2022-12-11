import { Component, OnInit } from '@angular/core';
import { Chat } from 'src/app/models/chat.model';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { ChatService } from 'src/app/services/chat.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-chats-list',
  templateUrl: './chats-list.component.html',
  styleUrls: ['./chats-list.component.css']
})
export class ChatsListComponent implements OnInit {
  currentUserId!: string;  
  chats?: Chat[];
  firstUser?: User;
  secondUser?: User;


  constructor(private chatService: ChatService,
    private authService: AuthService,
    private userService: UserService){} 

  ngOnInit(): void {
    this.currentUserId = this.authService.getUserId();
    this.getChats();
  }

  getChats(){
    this.chatService.getAll(this.currentUserId)
      .subscribe(chats => {
        this.chats = chats;
      });
  }
  
  deleteChat(id: string){
    this.chatService.delete(id).
      subscribe(() => window.location.reload());
  }
}
