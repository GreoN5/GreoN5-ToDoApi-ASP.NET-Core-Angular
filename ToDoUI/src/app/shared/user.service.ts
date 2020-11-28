import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private fb: FormBuilder, private http: HttpClient) { }

  readonly ApiURL = 'https://localhost:44346'; //URL to the server

  registerModel = this.fb.group({
    Username: new FormControl('', [Validators.required]),
    Passwords: this.fb.group({
      Password: new FormControl('', [Validators.required, Validators.minLength(6)]),
      ConfirmPassword: new FormControl('', [Validators.required])
    }, {
      validator: this.comparePasswords
    })
  });

  comparePasswords(fb: FormGroup) { //compare the password and confirmPassword from the registerModel
    let confirmPassword = fb.get('ConfirmPassword')

    if (confirmPassword.errors == null || 'passwordMismatch' in confirmPassword.errors) {
      if (fb.get('Password').value != confirmPassword.value) {
        confirmPassword.setErrors({ passwordMismatch: true });
      } else {
        confirmPassword.setErrors(null);
      }
    }
  }

  register() {
    var registrationUser = {
      Username: this.registerModel.value.Username,
      Password: this.registerModel.value.Passwords.Password
    } //making a new object user with the values of the form

    return this.http.post(this.ApiURL + '/User/Register', registrationUser); // return the response to the server url
  }

  login(data) {
    return this.http.post(this.ApiURL + '/User/Login', data);
  }
}
