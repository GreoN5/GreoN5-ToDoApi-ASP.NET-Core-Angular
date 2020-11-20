import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegistrationComponent } from './user/registration/registration.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { TodosComponent } from './user/todos/todos.component';
import { CreateItemComponent } from './user/todos/create-item/create-item.component';
import { AuthGuard } from './auth/auth.guard';

const routes: Routes = [
  { path: "", redirectTo: "/user/registration", pathMatch: "full" },
  {
    path: "user", component: UserComponent,
    children: [
      { path: "registration", component: RegistrationComponent },
      { path: "login", component: LoginComponent }
    ]
  },
  { path: "todo", component: TodosComponent, canActivate: [AuthGuard] },
  { path: "todo/create", component: CreateItemComponent, canActivate: [AuthGuard] }
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
