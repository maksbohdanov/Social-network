import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ProfileEditComponent } from './components/profile-edit/profile-edit.component';
import { ProfileComponent } from './components/profile/profile.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { UsersComponent } from './components/users/users.component';
import { FriendsListComponent } from './components/friends-list/friends-list.component';
import { FriendsRequestsComponent } from './components/friends-requests/friends-requests.component';
import { ChatsListComponent } from './components/chats-list/chats-list.component';
import { ChatComponent } from './components/chat/chat.component';
import { AdminGuard } from './guards/admin.guard';
import { UserGuard } from './guards/user.guard';


const routes: Routes =[
    { path: 'login', component: LoginComponent},
    { path: 'registration', component: RegistrationComponent},
    { path: 'profile/:id', component: ProfileComponent, canActivate: [UserGuard]},
    { path: 'profile/edit/:id', component: ProfileEditComponent, canActivate: [UserGuard]},
    { path: 'users', component: UsersComponent, canActivate: [AdminGuard]},
    { path: 'friends', component: FriendsListComponent, canActivate: [UserGuard]},
    { path: 'friends/requests', component: FriendsRequestsComponent, canActivate: [UserGuard]},
    { path: 'chats', component: ChatsListComponent, canActivate: [UserGuard]},
    { path: 'chats/:id', component: ChatComponent, canActivate: [UserGuard]},
];
  
@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
export class AppRoutingModule{}     