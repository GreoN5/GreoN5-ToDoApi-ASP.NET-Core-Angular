using System;
using System.Collections.Generic;
using System.Linq;
using ToDo.Data;
using ToDo.Models;
using ToDo.ViewModels;

namespace ToDo.Repositories
{
	public class ToDoRepository
	{
		private readonly ToDoContext _toDoContext;

		public ToDoRepository(ToDoContext context)
		{
			_toDoContext = context;
		}

		public List<ToDoDetailsVM> GetToDosByUser(string username)
		{
			if (DoesUserExist(username))
			{
				var toDoItems = _toDoContext.ToDos.Where(td => td.User.Username == username).ToList();

				for (int i = 0; i < toDoItems.Count; i++)
				{
					UpdateStatusDependingOnTimeLeft(toDoItems[i]);
				}

				var toDoDetails = toDoItems.Select(x => new ToDoDetailsVM
				{
					ID = x.ID,
					Name = x.Name,
					Description = x.Description,
					DueDate = x.DueDate,
					IsDone = x.IsDone,
					Status = x.ToDoStatus
				}).ToList();

				return toDoDetails;
			}

			return null;
		}

		public ToDoDetailsVM CreateNewTask(ToDoCreateModelVM createModel, string username)
		{
			if (DoesUserExist(username))
			{
				var toDo = new ToDoItem()
				{
					ID = createModel.ID,
					Name = createModel.Name,
					Description = createModel.Description,
					DueDate = DateTime.Now.AddHours(createModel.DueIn),
					IsDone = createModel.IsDone,
					User = GetUserByUsername(username)
				};

				UpdateStatusDependingOnTimeLeft(toDo); // sets the status to the new object

				_toDoContext.ToDos.Add(toDo);
				_toDoContext.SaveChanges();

				return new ToDoDetailsVM()
				{
					ID = toDo.ID,
					Name = createModel.Name,
					Description = createModel.Description,
					DueDate = DateTime.Now.AddHours(createModel.DueIn),
					IsDone = createModel.IsDone,
					Status = toDo.ToDoStatus
				};
			}

			return null;
		}

		public ToDoDetailsVM CompleteTask(int id, string username)
		{
			if (DoesUserExist(username))
			{
				var toDoTask = _toDoContext.ToDos.Where(td => td.ID == id && td.User.Username == username).FirstOrDefault();

				if (toDoTask == null)
				{
					return null;
				}

				toDoTask.IsDone = true;

				UpdateStatusDependingOnTimeLeft(toDoTask); // updates the status to true
				_toDoContext.SaveChanges();

				return new ToDoDetailsVM()
				{
					ID = toDoTask.ID,
					Name = toDoTask.Name,
					Description = toDoTask.Description,
					DueDate = toDoTask.DueDate,
					IsDone = toDoTask.IsDone,
					Status = toDoTask.ToDoStatus
				};
			}

			return null;
		}

		public bool DeleteTask(int id, string username)
		{
			if (DoesUserExist(username))
			{
				var toDo = GetTaskToDelete(id, username);

				if (toDo == null)
				{
					return false;
				}

				if (toDo.ToDoStatus != Status.Overdue && toDo.ToDoStatus != Status.Completed)
				{
					_toDoContext.ToDos.Remove(toDo);
					_toDoContext.SaveChanges();

					return true;
				}
			}

			return false;
		}

		private bool DoesUserExist(string username)
		{
			var user = _toDoContext.Users.Find(username);

			if (user != null)
			{
				return true;
			}

			return false;
		}

		private ToDoItem GetTaskToDelete(int id, string username)
		{
			var toDo = _toDoContext.ToDos.Where(td => td.ID == id && td.User.Username == username).FirstOrDefault();

			if (toDo == null)
			{
				return null;
			}

			return toDo;
		}

		private User GetUserByUsername(string username)
		{
			return _toDoContext.Users.Where(u => u.Username == username).FirstOrDefault();
		}

		private void UpdateStatusDependingOnTimeLeft(ToDoItem toDoItem)
		{
			var timeLeft = toDoItem.DueDate - DateTime.Now; // get the remaining time of the task to be done

			if (toDoItem.IsDone == false)
			{ // if the task is not completed
				if (timeLeft.TotalHours > 12)
				{
					toDoItem.ToDoStatus = Status.Incompleted;
				}
				else if (timeLeft.TotalHours < 12 && timeLeft.TotalHours > 0)
				{
					toDoItem.ToDoStatus = Status.Urgent;
				}
				else if (timeLeft.TotalHours < 0)
				{
					toDoItem.ToDoStatus = Status.Overdue;
				}
			} 
			else
			{
				toDoItem.ToDoStatus = Status.Completed;
			}
		}
	}
}
