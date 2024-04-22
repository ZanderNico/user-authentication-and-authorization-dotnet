using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UserAuth.Data.Enums;

namespace UserAuth.Models
{
	public class Users
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "Username is required")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Roles are required")]
		public UserRole Roles { get; set; }

	}
}
