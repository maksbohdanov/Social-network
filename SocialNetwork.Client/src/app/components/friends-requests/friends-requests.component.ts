import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { Friendship } from 'src/app/models/friendship.model';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-friends-requests',
  templateUrl: './friends-requests.component.html',
  styleUrls: ['./friends-requests.component.css']
})
export class FriendsRequestsComponent implements OnInit{
  currentUserId!: string;
  requests?: Friendship[];

  constructor(private userService: UserService,
    private authService: AuthService,
    private router: Router){}


  ngOnInit(): void {
    this.currentUserId = this.authService.getUserId();
    this.getRequests()  }
    
  getRequests(){
    this.userService.getRequests(this.currentUserId)
      .subscribe(requests  => this.requests = requests);
  }

  approve(id: string, answer: boolean){
    this.userService.approveFriendship(id, answer)
    .subscribe({
      next: () => {
        this.router.navigate([`friends`]);
      }
    });
  }
}
