using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Data;
using ToDo.Extensions;
using ToDo.Models;
using ToDo.Repositories;
using ToDo.ViewModels;

namespace ToDo.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize(Roles = "User")]
	public class ToDosController : Controller
	{
		private readonly ToDoContext _toDoContext;
		private ToDoRepository _repository;
		private List<ToDoDetailsVM> _toDoDetails = new List<ToDoDetailsVM>();

		public ToDosController(ToDoContext context)
		{
			_toDoContext = context;
		}

		[Route("{username}/ToDoItems")]
		[HttpGet]
		public IActionResult GetAllTasksByUser(string username)
		{
			if (DoesUserExist(username))
			{
				_repository = new ToDoRepository(_toDoContext);
				_toDoDetails = _repository.GetToDosByUser(username);

				return Ok(_toDoDetails);
			}

			return NotFound();
		}

		[Route("{username}/CreateToDo")]
		[HttpPost]
		public IActionResult CreateTask(ToDoCreateModelVM createModel, string username)
		{
			if (DoesUserExist(username))
			{
				if (!ModelState.IsValid)
				{
					return BadRequest();
				}

				_repository = new ToDoRepository(_toDoContext);
				var toDoDetails = _repository.CreateNewTask(createModel, username);

				return Ok(toDoDetails);
			}

			return NotFound();
		}

		[Route("{username}/CompleteTask/{id}")]
		[HttpPut]
		public IActionResult CompleteTask(int id, string username)
		{
			if (DoesUserExist(username))
			{
				_repository = new ToDoRepository(_toDoContext);
				var toDoTask = _repository.CompleteTask(id, username);

				if (toDoTask == null)
				{
					return NotFound();
				}

				return Ok(toDoTask);
			}

			return NotFound();
		}

		[Route("{username}/DeleteToDo/{id}")]
		[HttpDelete]
		public IActionResult DeleteTask(int id, string username)
		{
			if (DoesUserExist(username))
			{
				_repository = new ToDoRepository(_toDoContext);

				if (_repository.DeleteTask(id, username))
				{
					return Ok("Task successfully deleted!");
				}

				return NotFound();
			}

			return NotFound();
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
	}
}
