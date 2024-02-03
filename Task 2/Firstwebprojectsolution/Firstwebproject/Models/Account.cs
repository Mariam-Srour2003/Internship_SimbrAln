using System.ComponentModel.DataAnnotations;

namespace Firstwebproject.Models
{

	public class Account
	{
		[Key]
		public int AccountId { get; set; }

		[Required(ErrorMessage ="Password is Required!")]
		public string? Password { get; set; }

		[Required(ErrorMessage = "New Password is Required!")]
		public string? OldPassword { get; set; }
	}
}