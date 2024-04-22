using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using UserAuth.Dtos;
using UserAuth.Interfaces;
using UserAuth.Models;

namespace UserAuth.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : Controller
	{
		private readonly IUsersRepository _usersRepository;
		private readonly IMapper _mapper;

		public UsersController(IUsersRepository usersRepository, IMapper mapper)
        {
			_usersRepository = usersRepository;
			_mapper = mapper;
		}

		[HttpPost]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> CreateUser(UsersDto usersCreate)
		{
			if (usersCreate == null)
				return BadRequest(ModelState);

			// Check model state validity
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			// Map DTO to entity
			var user = _mapper.Map<Users>(usersCreate);

			// Add user
			var addUserResult = await _usersRepository.AddUserAsync(user);
			if (!addUserResult)
			{
				ModelState.AddModelError("", "Something went wrong while saving");
				return StatusCode(500, ModelState);
			}

			return Ok("Successfully created");
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		[ProducesResponseType(403)]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _usersRepository.GetAllUsersAsync();
			if (users == null || users.Count == 0)
				return NotFound();

			var usersDto = _mapper.Map<List<UsersDto>>(users);
			return Ok(usersDto);
		}


		[HttpGet("{userId}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetUserById(int userId)
		{
			if (!_usersRepository.UserExists(userId))
				return NotFound();

			var user = await _usersRepository.GetUserByIdAsync(userId);

			var userDto = _mapper.Map<UsersDto>(user);
			return Ok(userDto);
		}

		[HttpPost("login")]
		[ProducesResponseType(200)]
		public async Task<IActionResult> Login(LoginDto loginDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var token = await _usersRepository.LoginUsers(loginDto.Email, loginDto.Password);
			if (token == null)
				return Unauthorized();

			return Ok(new { token });
		}

		[HttpPut("{userId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> UpdateUser(int userId, [FromBody] UsersDto updateUser)
		{
			if (updateUser == null)
				return BadRequest(ModelState);

			if (userId != updateUser.Id)
				return BadRequest(ModelState);

			if (!_usersRepository.UserExists(userId))
				return NotFound();

			if (!ModelState.IsValid)
				return BadRequest();


			var user = _mapper.Map<Users>(updateUser);

			var updated = await _usersRepository.UpdateUserAsync(user);
			if (!updated)
				return StatusCode(500, "Failed to update user");

			return NoContent();
		}

		[HttpDelete("{userId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> DeleteUser(int userId)
		{
			var user = await _usersRepository.GetUserByIdAsync(userId);
			if (!_usersRepository.UserExists(userId))
				return NotFound();

			var deleted = await _usersRepository.DeleteUserAsync(user);
			if (!deleted)
				return StatusCode(500, "Failed to delete user");

			return NoContent();
		}

	}
}
