﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Data;
using ToDo.Extensions;
using ToDo.Models;
using ToDo.ViewModels;

namespace ToDo.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize(Roles = "User")]
	public class ToDos : Controller
	{
		private ToDoContext _toDoContext = new ToDoContext();
		private List<ToDoDetailsVM> _toDoDetails = new List<ToDoDetailsVM>();
		private List<ToDoItem> _toDoItems = new List<ToDoItem>();

		[Route("{username}/ToDoItems")]
		[HttpGet]
		public IActionResult GetAllTasksByUser(string username)
		{
			User user = FindUserByUsername(username);

			if (user != null)
			{
				this._toDoItems = this._toDoContext.ToDos.Where(td => td.User.Username == username).ToList();
				for (int i = 0; i < this._toDoItems.Count; i++)
				{
					this._toDoItems[i].UpdateStatusDependingOnTimeLeft();
				}

				this._toDoDetails = this._toDoItems.Select(x => new ToDoDetailsVM
				{
					ID = x.ID,
					Name = x.Name,
					Description = x.Description,
					DueDate = x.DueDate,
					IsDone = x.IsDone,
					Status = x.ToDoStatus
				}).ToList();

				return Ok(this._toDoDetails);
			}

			return NotFound(user);
		}

		[Route("{username}/CreateToDo")]
		[HttpPost]
		public IActionResult CreateTask(ToDoCreateModelVM createModel, string username)
		{
			User user = FindUserByUsername(username);

			if (user != null)
			{
				if (!ModelState.IsValid)
				{
					return BadRequest();
				}

				var toDo = new ToDoItem()
				{
					ID = createModel.ID,
					Name = createModel.Name,
					Description = createModel.Description,
					DueDate = DateTime.Now.AddHours(createModel.DueIn),
					IsDone = createModel.IsDone,
					User = user
				};

				toDo.UpdateStatusDependingOnTimeLeft(); // sets the status to the new object

				this._toDoContext.ToDos.Add(toDo);
				this._toDoContext.SaveChanges();

				var toDoDetails = new ToDoDetailsVM()
				{
					ID = toDo.ID,
					Name = createModel.Name,
					Description = createModel.Description,
					DueDate = DateTime.Now.AddHours(createModel.DueIn),
					IsDone = createModel.IsDone
				};

				return Ok(toDoDetails);
			}

			return NotFound(user);
		}

		[Route("{username}/CompleteTask/{id}")]
		[HttpPut]
		public IActionResult CompleteTask(int id, string username)
		{
			User user = FindUserByUsername(username);

			if (user != null)
			{
				var toDoTask = this._toDoContext.ToDos.Where(td => td.ID == id && td.User.Username == username).FirstOrDefault();

				if (toDoTask == null)
				{
					return NotFound();
				}

				toDoTask.IsDone = true;

				toDoTask.UpdateStatusDependingOnTimeLeft(); // updates the status to true

				this._toDoContext.SaveChanges();

				return Ok(toDoTask);
			}

			return NotFound(user);
		}

		[Route("{username}/DeleteToDo/{id}")]
		[HttpDelete]
		public IActionResult DeleteTask(int id, string username)
		{
			User user = FindUserByUsername(username);

			if (user != null)
			{
				var toDo = this._toDoContext.ToDos.Where(td => td.ID == id && td.User.Username == username).FirstOrDefault();

				if (toDo == null)
				{
					return NotFound(toDo);
				}

				var toDoTask = this._toDoContext.ToDos.Where(td => td.ID == id)
													  .Select(td => new ToDoDetailsVM
													  {
														  ID = td.ID,
														  Name = td.Name,
														  Description = td.Description,
														  DueDate = td.DueDate,
														  IsDone = td.IsDone
													  })
													  .FirstOrDefault();

				if (toDo.ToDoStatus != "Overdue" && toDo.ToDoStatus != "Completed")
				{
					this._toDoContext.ToDos.Remove(toDo);
					this._toDoContext.SaveChanges();

					return Ok();
				}
			}

			return NotFound(user);
		}

		private User FindUserByUsername(string userName)
		{
			return this._toDoContext.Users.Where(u => u.Username == userName).FirstOrDefault();
		}
	}
}
