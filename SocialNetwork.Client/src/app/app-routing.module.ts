import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegistrationComponent } from './components/registration/registration.component';

const routes: Routes =[
    // {
    //     path: '',
    //     component: HomeComponent
    // },
    { path: 'login', component: LoginComponent},
    { path: 'registration', component: RegistrationComponent},
    // {
    //     path: 'threads',
    //     component: HomeComponent
    // },
   
    // {
    //     path: 'threads/:id',
    //     component: ForumThreadComponent
    // },
    // {
    //     path: 'users/:id',
    //     component: UserComponent
    // },
    // {
    //     path: 'roles',
    //     component: RoleListComponent, 
    //     canActivate: [HasRoleGuard],
    //     data:{
    //         role: 'admin'
    //     }
    // },
    // {
    //     path: 'users',
    //     component: UserListComponent, 
    //     canActivate: [HasRoleGuard],
    //     data:{
    //         role: 'admin'
    //     }
    // },
    // {
    //     path: 'themes',
    //     component: ThemeListComponent,
    //     canActivate: [HasRoleGuard],
    //     data:{
    //         role: ['admin', 'moderator']
    //     }
    // },
    // {
    //     path: 'threads/edit/:id',
    //     component: ThreadUpdateComponent,
    //     canActivate: [HasRoleGuard],
    //     data:{
    //         role: ['admin', 'moderator']
    //     }
    // },
    // {
    //     path: 'posts/edit/:id',        
    //     component: PostUpdateComponent,
    //     canActivate: [HasRoleGuard],
    //     data:{
    //         role: ['admin', 'moderator']
    //     }
    // },
];
  
@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
export class AppRoutingModule{}     