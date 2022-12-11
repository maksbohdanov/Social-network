import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit{
  users?: User[];

  constructor(private userService: UserService){}
  ngOnInit(): void {
    this.getUsers();
  }


  getUsers(){
    this.userService.getAll()
      .subscribe(users => this.users = users);
  }
  
    

}
