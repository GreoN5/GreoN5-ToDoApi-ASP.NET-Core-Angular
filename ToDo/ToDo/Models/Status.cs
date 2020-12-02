using System.ComponentModel;

namespace ToDo.Models
{
	public enum Status
	{
		[Description("Incompleted")]
		Incompleted = 1,

		[Description("Urgent")]
		Urgent = 2,

		[Description("Overdue")]
		Overdue = 3,

		[Description("Completed")]
		Completed = 4
	}
}
