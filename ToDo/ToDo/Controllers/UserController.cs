using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ToDo.Data;
using ToDo.Models;
using ToDo.Repositories;
using ToDo.ViewModels;

namespace ToDo.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[AllowAnonymous]
	public class UserController : Controller
	{
		private UserRepository _repository;

		public UserController(UserRepository repository)
		{
			_repository = repository;
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("Register")]
		public IActionResult Register([FromBody] UserVM registrationUser)
		{
			var user = _repository.Registration(registrationUser);

			if (user == null)
			{
				return StatusCode(409, $"User with {registrationUser.Username} already exists!");
			}

			return Ok(user);
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("Login")]
		public IActionResult Login([FromBody] UserVM loginUser)
		{
			User user = _repository.Login(loginUser);

			if (user != null)
			{
				var tokenString = _repository.GenerateJWTToken(user);

				return Ok(new { token = tokenString, userDetails = user });
			}

			return NotFound("User not found!");
		}
	}
}
