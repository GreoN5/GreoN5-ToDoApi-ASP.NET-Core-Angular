import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  constructor(public service: UserService) { }

  ngOnInit(): void {
    this.service.registerModel.reset();
  }

  onSubmit() {
    this.service.register().subscribe(
      (response: any) => { //in any kind of response it will clear the form
        this.service.registerModel.reset();
      },
      error => {
        console.log(error);
      }
    )
  }
}
