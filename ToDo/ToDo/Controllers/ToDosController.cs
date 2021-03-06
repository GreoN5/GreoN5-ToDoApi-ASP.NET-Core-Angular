﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Repositories;
using ToDo.ViewModels;

namespace ToDo.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize(Roles = "User")]
	public class ToDosController : Controller
	{
		private ToDoRepository _repository;
		private List<ToDoDetailsVM> _toDoDetails = new List<ToDoDetailsVM>();

		public ToDosController(ToDoRepository repository)
		{
			_repository = repository;
		}

		[Route("{username}/ToDoItems")]
		[HttpGet]
		public IActionResult GetAllTasksByUser(string username)
		{
			_toDoDetails = _repository.GetToDosByUser(username);

			if (_toDoDetails == null)
			{
				return NotFound("User not found!");
			}

			return Ok(_toDoDetails);
		}

		[Route("{username}/CreateToDo")]
		[HttpPost]
		public IActionResult CreateTask(ToDoCreateModelVM createModel, string username)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var toDoDetails = _repository.CreateNewTask(createModel, username);

			if (toDoDetails == null)
			{
				return NotFound("User not found!");
			}

			return Ok(toDoDetails);
		}

		[Route("{username}/CompleteTask/{id}")]
		[HttpPut]
		public IActionResult CompleteTask(int id, string username)
		{
			var toDoTask = _repository.CompleteTask(id, username);

			if (toDoTask == null)
			{
				return NotFound("User or task not found!");
			}

			return Ok(toDoTask);
		}

		[Route("{username}/DeleteToDo/{id}")]
		[HttpDelete]
		public IActionResult DeleteTask(int id, string username)
		{
			if (_repository.DeleteTask(id, username))
			{
				return Ok("Task successfully deleted!");
			}

			return NotFound();
		}
	}
}
