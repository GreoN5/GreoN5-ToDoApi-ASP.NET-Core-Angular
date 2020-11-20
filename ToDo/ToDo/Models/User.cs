using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Models
{
	public class User
	{
		[Key]
		[Column(TypeName = "varchar(100)")]
		public string Username { get; set; }

		[Column(TypeName = "varchar(100)")]
		public string Password { get; set; }

		[Column(TypeName = "varchar(50)")]
		public string UserRole { get; set; }

		public List<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();
	}
}
