import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class TodoService {

  readonly ApiURL = "https://localhost:44346"; //URL to the server

  constructor(private http: HttpClient, private fb: FormBuilder) { }

  createModel = this.fb.group({
    Name: new FormControl("", [Validators.required, Validators.maxLength(100)]),
    Description: new FormControl("", [Validators.maxLength(500)]),
    DueIn: new FormControl(null, [Validators.required, Validators.min(1)])
  });

  toDoList = [];

  checkDueIn(fb: FormControl) {
    let dueIn = fb.get("DueIn");

    if (dueIn.errors == null && dueIn.value <= 0) {
      dueIn.setErrors({ invalidTime: true });
    } else {
      dueIn.setErrors(null);
    }
  }

  getToDosByUser(): any {
    return this.http.get(this.ApiURL + "/ToDos/" + this.getLoggedUser() + "/ToDoItems")
  }

  createToDo() {
    var toDoItem = {
      Name: this.createModel.value.Name,
      Description: this.createModel.value.Description,
      DueIn: this.createModel.value.DueIn
    }

    return this.http.post(this.ApiURL + "/ToDos/" + this.getLoggedUser() + "/CreateToDo", toDoItem);
  }

  updateToDo(id) {
    return this.http.put(this.ApiURL + "/ToDos/" + this.getLoggedUser() + "/UpdateToDo/" + id, id);
  }

  completeToDo(id) {
    return this.http.put(this.ApiURL + "/ToDos/" + this.getLoggedUser() + "/CompleteTask/" + id, id);
  }

  deleteToDo(id) {
    return this.http.delete(this.ApiURL + "/ToDos/" + this.getLoggedUser() + "/DeleteToDo/" + id);
  }

  getLoggedUser() {
    return localStorage.getItem("loggedUser"); // get the logged user from the local storage
  }
}
