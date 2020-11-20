using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Models
{
	public class ToDoItem
	{
		[Key]
		public int ID { get; set; }

		[Column(TypeName = "varchar(100)")]
		[Required]
		public string Name { get; set; }

		[Column(TypeName = "varchar(500)")]
		public string Description { get; set; }

		[Column(TypeName = "datetime")]
		public DateTime DueDate { get; set; }

		[Column(TypeName = "varchar(50)")]
		public string ToDoStatus { get; set; }

		[Column(TypeName = "bit")]
		public bool IsDone { get; set; }

		public User User { get; set; }
	}
}
