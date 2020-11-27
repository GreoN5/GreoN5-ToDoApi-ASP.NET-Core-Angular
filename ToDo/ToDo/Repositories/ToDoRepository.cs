using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Data;
using ToDo.Extensions;
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
			var toDoItems = _toDoContext.ToDos.Where(td => td.User.Username == username).ToList();

			for (int i = 0; i < toDoItems.Count; i++)
			{
				toDoItems[i].UpdateStatusDependingOnTimeLeft();
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

		public ToDoDetailsVM CreateNewTask(ToDoCreateModelVM createModel, string username)
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

			toDo.UpdateStatusDependingOnTimeLeft(); // sets the status to the new object

			_toDoContext.Add(toDo);
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

		public ToDoDetailsVM CompleteTask(int id, string username)
		{
			var toDoTask = _toDoContext.ToDos.Where(td => td.ID == id && td.User.Username == username).FirstOrDefault();

			if (toDoTask == null)
			{
				return null;
			}

			toDoTask.IsDone = true;

			toDoTask.UpdateStatusDependingOnTimeLeft(); // updates the status to true
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

		public bool DeleteTask(int id, string username)
		{
			var toDo = GetTaskToDelete(id, username);

			if (toDo == null)
			{
				return false;
			}

			if (toDo.ToDoStatus != "Overdue" && toDo.ToDoStatus != "Completed")
			{
				_toDoContext.ToDos.Remove(toDo);
				_toDoContext.SaveChanges();

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
	}
}
