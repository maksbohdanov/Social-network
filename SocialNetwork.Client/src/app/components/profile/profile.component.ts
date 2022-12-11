import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit{
 
  currentUserId!: string;
  userId!: string;
  user!: User;
  isAdmin!: boolean;

  constructor(private router: Router,
     private userService: UserService,
      private route: ActivatedRoute,
      private authService: AuthService,
      private notificationService: NotificationService,) {}

  ngOnInit(): void {
    this.currentUserId = this.authService.getUserId();
    this.route.params.subscribe(params => {
      this.userId = params['id']; 
      this.getUser(this.userId);
    });
  
    this.isAdmin = this.authService.isAdmin   
  }

  public getUser(id: string){
    this.userService.getById(id)
    .subscribe({
      next: res =>{
        if(res)
          this.user = res} ,
      error: err => {
        this.notificationService.notifyError(`Loading data failed. ${err.error?.message ?? ''}`, true);        
      }
    })
  }   
  
  public navigateToEditPage(){
    this.router.navigate(['profile/edit/' + this.userId]);
  }

  public deleteUser(id: string){
    this.userService.delete(id).
      subscribe({
        next: () => this.router.navigate(['users']),
        error: err => {
          this.notificationService.notifyError(`Cannot delete user. ${err.error?.message ?? ''}`, true);        
        }
      })
  }
}
