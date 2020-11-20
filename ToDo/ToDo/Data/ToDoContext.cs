﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Data
{
	public class ToDoContext : DbContext
	{
		public DbSet<ToDoItem> ToDos { get; set; }
		public DbSet<User> Users { get; set; }

		public ToDoContext() : base() { }

		protected override void OnConfiguring(DbContextOptionsBuilder builder)
		{
			builder.UseSqlServer("Server=DESKTOP-OI3QR52\\SQLEXPRESS;Database=ToDos;Trusted_Connection=True;MultipleActiveResultSets=True;");
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<User>().HasKey(u => u.Username);
			builder.Entity<User>().Property(u => u.Username).ValueGeneratedNever();
		}
	}
}
