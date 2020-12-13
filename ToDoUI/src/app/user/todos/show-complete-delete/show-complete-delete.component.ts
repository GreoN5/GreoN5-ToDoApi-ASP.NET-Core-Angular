import { Component, OnInit } from '@angular/core';
import { TodoService } from 'src/app/shared/todo.service';

@Component({
  selector: 'app-show-complete-delete',
  templateUrl: './show-complete-delete.component.html',
  styleUrls: ['./show-complete-delete.component.css']
})
export class ShowCompleteDeleteComponent implements OnInit {

  constructor(public todoService: TodoService) { }

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
            alert('Item is successfully completed!');

            break;
          }
        }

        this.showToDos(); // refreshes the page
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
            alert('Item is successfully deleted!');

            break; //no need to go through another loop of the item is found
          }
        }

        this.showToDos(); // refreshes the page
      }, error => {
        console.log(error);
      }
    )
  }

}
