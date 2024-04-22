using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAuth.Interfaces;
using UserAuth.Models;

namespace UserAuth.Services
{
	public class AuthService : IAuthService
	{
		private readonly IConfiguration _config;

		public AuthService(IConfiguration configuration)
		{
			_config = configuration;
		}

		public async Task<string> GenerateTokenAsync(Users user)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.Roles.ToString()),
				new Claim("user_id", user.Id.ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				_config.GetSection("Jwt:Secret").Value!));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: creds
				);
			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
			
		}
	}
}
