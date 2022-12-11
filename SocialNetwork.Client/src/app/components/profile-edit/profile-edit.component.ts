import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile-edit',
  templateUrl: './profile-edit.component.html',
  styleUrls: ['./profile-edit.component.css']
})
export class ProfileEditComponent {
  currentUserId!: string;
  userId!: string;
  user!: User
  isAdmin!: boolean;
  
  isError: boolean = false;
  isPasswordError: boolean = false;
  isPasswordSuccess: boolean = false;
  isSuccess: boolean = false;
  isRepeatError: boolean = false;
  
  repeatPassword: string = '';

  constructor(private router: Router,
    private userService: UserService, 
    private authService: AuthService,
    private route: ActivatedRoute,
    private notificationService: NotificationService) {
  }

  public ngOnInit(): void {
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

  onSubmit() {
    this.userService.edit(this.user)
      .subscribe({
        next: () => {
          window.location.reload();
        },
        error: err => {
          this.notificationService.notifyError(`Registration failed. ${err.error?.Message ?? ''}`);
        }
      });
  }
  
  isValidDate(date: Date){
    let a = date.getUTCDate;
    let b = Date.now
    let c = a < b;
    return date.getUTCDate < Date.now
  }
}
