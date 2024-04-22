using UserAuth.Models;

namespace UserAuth.Interfaces
{
	public interface IUsersRepository
	{
		Task<Users> GetUserByIdAsync(int userId);
		Task<ICollection<Users>> GetAllUsersAsync();
		Task<bool> AddUserAsync(Users user);
		Task<bool> DeleteUserAsync(Users user);
		Task<bool> UpdateUserAsync(Users user);
		Task<string> LoginUsers(string email, string password);
		bool Save();
		bool UserExists(int userId);
	}
}
