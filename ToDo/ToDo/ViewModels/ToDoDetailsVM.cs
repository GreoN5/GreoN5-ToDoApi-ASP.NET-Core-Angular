using System;
using ToDo.Models;

namespace ToDo.ViewModels
{
	public class ToDoDetailsVM
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime DueDate { get; set; }

		public bool IsDone { get; set; }

		public Status Status { get; set; }
	}
}
