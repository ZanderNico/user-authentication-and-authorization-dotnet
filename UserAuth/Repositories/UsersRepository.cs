using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
using UserAuth.Helpers;
using UserAuth.Interfaces;
using UserAuth.Models;

namespace UserAuth.Repositories
{
	public class UsersRepository : IUsersRepository
	{
		private readonly DataContext _context;
		private readonly IAuthService _authService;

		public UsersRepository(DataContext context, IAuthService authService)
        {
			_context = context;
			_authService = authService;
		}

		public async Task<bool> AddUserAsync(Users user)
		{
			if (PasswordHashing.IsPasswordComplex(user.Password))
			{
				user.Password = PasswordHashing.HashPassword(user.Password);
				await _context.Users.AddAsync(user);
				return Save();
			}
			else
			{
				return false;
			}
		}

		public async Task<bool> DeleteUserAsync(Users user)
		{
			_context.Users.Remove(user);
			return Save();
		}

		public async Task<ICollection<Users>> GetAllUsersAsync()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<Users> GetUserByIdAsync(int userId)
		{
			return await _context.Users.FindAsync(userId);
		}

		public async Task<string> LoginUsers(string email, string password)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

			if (user == null)
			{
				// User not found
				return null;
			}

			if (!PasswordHashing.VerifyPassword(password, user.Password))
			{
				// Incorrect password
				return null;
			}

			var token = await _authService.GenerateTokenAsync(user);
			return token;
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public async Task<bool> UpdateUserAsync(Users user)
		{
			if (!string.IsNullOrWhiteSpace(user.Password))
			{
				// Check if the password meets the complexity requirements
				if (!PasswordHashing.IsPasswordComplex(user.Password))
				{
					return false;
				}

				// Hash the password
				user.Password = PasswordHashing.HashPassword(user.Password);
			}

			_context.Users.Update(user);
			return Save();
		}

		public bool UserExists(int userId)
		{
			return _context.Users.Any(u => u.Id == userId);
		}
	}
}
