import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TodoService } from 'src/app/shared/todo.service';

@Component({
  selector: 'app-create-item',
  templateUrl: './create-item.component.html',
  styleUrls: ['./create-item.component.css']
})
export class CreateItemComponent implements OnInit {

  constructor(public todoService: TodoService, private router: Router) { }

  ngOnInit(): void {
  }

  onSubmit() {
    this.todoService.createToDo().subscribe(
      response => {
        this.todoService.createModel.reset();
        alert("Item is successfully created!");
        this.router.navigateByUrl("/todo");
      },
      error => {
        console.log(error);
      }
    )
  }
}
