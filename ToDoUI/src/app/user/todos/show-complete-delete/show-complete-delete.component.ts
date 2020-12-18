import { Component, OnInit } from '@angular/core';
import { TodoService } from 'src/app/shared/todo.service';

@Component({
  selector: 'app-show-complete-delete',
  templateUrl: './show-complete-delete.component.html',
  styleUrls: ['./show-complete-delete.component.css']
})
export class ShowCompleteDeleteComponent implements OnInit {

  constructor(public todoService: TodoService) { }

  completeItemMsg: boolean
  deleteItemMsg: boolean

  ngOnInit(): void {
    this.showToDos();
  }

  showToDos() {
    this.todoService.getToDosByUser().subscribe(
      data => {
        this.todoService.toDoList = data;
      }, error => {
        console.log(error);
      });
  }

  completeTask(id) {
    this.todoService.completeToDo(id).subscribe(
      data => {
        for (let i = 0; i < this.todoService.toDoList.length; i++) {
          if (this.todoService.toDoList[i].id == id) {
            this.todoService.toDoList[i].isDone = true;

            break;
          }
        }

        this.showToDos(); // refreshes the page
        this.completeItemMsg = true
      }, error => {
        console.log(error);
      }
    )
  }

  deleteTask(id) {
    this.todoService.deleteToDo(id).subscribe(
      data => {
        for (let i = 0; i < this.todoService.toDoList.length; i++) {
          if (this.todoService.toDoList[i].id == id) {
            this.todoService.toDoList.splice(i, 1); // removes only the item with the particular id

            break; //no need to go through another loop of the item is found
          }
        }

        this.showToDos(); // refreshes the page
        this.deleteItemMsg = true
      }, error => {
        console.log(error);
      }
    )
  }

}
