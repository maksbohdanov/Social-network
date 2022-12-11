import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Friendship } from 'src/app/models/friendship.model';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { ChatService } from 'src/app/services/chat.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-friends-list',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.css']
})
export class FriendsListComponent  implements OnInit{  
  currentUserId!: string;  
  friends?: User[];
  filteredFriends?: User[];
  users?: User[];
  requests?: Friendship[];
  filterBy = '';
  buttonDisabled = false;


  constructor(private userService: UserService,
    private authService: AuthService,
    private chatService: ChatService,
    private router: Router){}  

  ngOnInit(): void {
    this.currentUserId = this.authService.getUserId();
    this.getFriends();
    this.getRequests()
  }

  getFriends(){
    this.userService.getFriends(this.currentUserId)
      .subscribe(users => {
        this.friends = users;
        this.filteredFriends = [...this.friends];
      });
  }

  addToFriends(friendId: string){
    this.userService.addToFriends(this.currentUserId,friendId)
      .subscribe(() => this.buttonDisabled = true);
  }

  getRequests(){
    this.userService.getRequests(this.currentUserId)
      .subscribe(requests  => this.requests = requests);
  }

  findUsers(){
    this.userService.fingByFilter(this.filterBy)
      .subscribe(users => this.users = users);
  }

  openChat(userId: string){
    this.chatService.create(this.currentUserId, userId)
      .subscribe({
        next: chat => {
          this.router.navigate(['chats/' + chat.id])
        }
      })
  }

  filter() {
    this.filteredFriends = [...this.friends!.filter(friend => 
      friend.firstName.toLocaleLowerCase().includes(this.filterBy.toLocaleLowerCase()) || 
      friend.lastName.toLocaleLowerCase().includes(this.filterBy.toLocaleLowerCase()) ||
      friend.city.toLocaleLowerCase().includes(this.filterBy.toLocaleLowerCase()))];
      this.users = [];
  }
}
