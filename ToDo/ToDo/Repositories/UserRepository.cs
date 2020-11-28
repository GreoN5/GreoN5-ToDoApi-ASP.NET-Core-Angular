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

namespace ToDo.Repositories
{
	public class UserRepository
	{
		private readonly IConfiguration _configuration;
		private readonly ToDoContext _toDoContext;

		public UserRepository(IConfiguration config, ToDoContext context)
		{
			_configuration = config;
			_toDoContext = context;
		}

		public User Registration(UserVM registrationUser)
		{
			var users = _toDoContext.Users.ToList();

			User newUser = new User()
			{
				Username = registrationUser.Username,
				Password = registrationUser.Password,
				UserRole = "User"
			};

			for (int i = 0; i < users.Count; i++)
			{
				if (newUser.Username == users[i].Username) // it makes the usernames unique
				{
					return null;
				}
			}

			_toDoContext.Users.Add(newUser);
			_toDoContext.SaveChanges();

			return newUser;
		}

		public User Login(UserVM loginUser)
		{
			User user = _toDoContext.Users.Where(u => u.Username == loginUser.Username && u.Password == loginUser.Password).FirstOrDefault();

			if (user != null)
			{
				return user;
			}

			return null;
		}

		public string GenerateJWTToken(User userToken)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, userToken.Username),
				new Claim("role", userToken.UserRole),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			};

			var token = new JwtSecurityToken(issuer: _configuration["Jwt:Issuer"], audience: _configuration["Jwt:Audience"], claims: claims, expires: DateTime.Now.AddHours(1), signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
