using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ToDo.Data;
using ToDo.Models;
using ToDo.ViewModels;

namespace ToDo.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[AllowAnonymous]
	public class UserController : Controller
	{
		private readonly IConfiguration _config;
		private readonly ToDoContext _context;

		public UserController(IConfiguration config, ToDoContext context)
		{
			_config = config;
			_context = context;
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("Register")]
		public IActionResult Register([FromBody] UserVM registrationUser)
		{
			var users = _context.Users.ToList();

			User user = new User()
			{
				Username = registrationUser.Username,
				Password = registrationUser.Password,
				UserRole = "User"
			};

			for (int i = 0; i < users.Count; i++)
			{
				if (user.Username == users[i].Username)
				{
					return StatusCode(409, $"User with {user.Username} already exists!"); // it makes the usernames unique
				}
			}

			_context.Users.Add(user);
			_context.SaveChanges();

			return Ok(user);
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("Login")]
		public IActionResult Login([FromBody] UserVM loginUser)
		{
			User user = _context.Users.Where(u => u.Username == loginUser.Username && u.Password == loginUser.Password).FirstOrDefault();

			if (user != null)
			{
				var tokenString = GenerateJWTToken(user);

				return Ok(new { token = tokenString, userDetails = user });
			}

			return NotFound(user);
		}

		private string GenerateJWTToken(User userInfo)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
				new Claim("role", userInfo.UserRole),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			};

			var token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"], audience: _config["Jwt:Audience"], claims: claims, expires: DateTime.Now.AddHours(1), signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
