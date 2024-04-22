using UserAuth.Models;

namespace UserAuth.Interfaces
{
    public interface IAuthService
    {
		Task<string> GenerateTokenAsync(Users user);
	}
}
