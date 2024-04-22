using System.ComponentModel.DataAnnotations;
using UserAuth.Data.Enums;

namespace UserAuth.Dtos
{
	public class UsersDto
	{
		[Key]
		public int Id { get; set; }
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; }
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public string Username { get; set; }
		public UserRole Roles { get; set; }
	}
}
