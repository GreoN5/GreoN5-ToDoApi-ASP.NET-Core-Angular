using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Extensions
{
	public static class ToDoItemExtensions
	{
		public static void UpdateStatusDependingOnTimeLeft(this ToDoItem toDoItem)
		{
			var timeLeft = toDoItem.DueDate - DateTime.Now; // get the remaining time of the task to be done

			if (toDoItem.IsDone == false)
			{ // if the task is not completed
				if (timeLeft.TotalHours > 12)
				{
					toDoItem.ToDoStatus = "Incompleted";
				}
				else if (timeLeft.TotalHours < 12 && timeLeft.TotalHours > 0)
				{
					toDoItem.ToDoStatus = "Urgent";
				}
				else if (timeLeft.TotalHours < 0)
				{
					toDoItem.ToDoStatus = "Overdue";
				}
			}
			else
			{ // if the task is completed
				toDoItem.ToDoStatus = "Completed";
			}
		}
	}
}
