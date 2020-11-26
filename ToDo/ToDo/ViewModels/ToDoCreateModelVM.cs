using System.ComponentModel.DataAnnotations;

namespace ToDo.ViewModels
{
	public class ToDoCreateModelVM
	{
		public int ID { get; set; }

		[StringLength(100, ErrorMessage = "The \"Name\" value cannot exceed 100 characters!")]
		[Required(ErrorMessage = "This attribute is required!")]
		public string Name { get; set; }

		[StringLength(500, ErrorMessage = "The \"Description\" value cannot exceed 500 characters!")]
		public string Description { get; set; }

		[Required(ErrorMessage = "This attribute is required!")]
		public int DueIn { get; set; }

		public bool IsDone { get; } // the new ToDoItem receives false as default value
									// assuming that the person enters a ToDo that is not finished yet
	}
}
