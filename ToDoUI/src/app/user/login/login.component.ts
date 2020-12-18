import { Component, OnInit } from '@angular/core';
import { NgForm, FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { error } from 'protractor';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginModel = {
    Username: '',
    Password: ''
  }

  constructor(public service: UserService, private router: Router) { }

  ngOnInit(): void {
    if (localStorage.getItem('token') != null) {
      this.router.navigateByUrl('/todo');
    }
  }

  onSubmit(loginForm: NgForm) {
    this.service.login(loginForm.value).subscribe(
      (response: any) => {
        localStorage.setItem('loggedUser', loginForm.value.Username); // sets the username in the local storage
        localStorage.setItem('token', response.token);
        this.router.navigateByUrl('/todo');
      }, error => {
        console.log(error);
      }
    );
  }
}
