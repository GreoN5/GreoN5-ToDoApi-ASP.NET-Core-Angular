using System.ComponentModel.DataAnnotations;

namespace ToDo.ViewModels
{
	public class UserLoginVM
	{
		[Required(ErrorMessage = "This field is required!")]
		public string Username { get; set; }

		[Required(ErrorMessage = "This field is required!")]
		public string Password { get; set; }
	}
}
