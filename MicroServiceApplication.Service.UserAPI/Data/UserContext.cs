using MicroServiceApplication.Service.UserAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MicroServiceApplication.Service.UserAPI.Data
{
	public class UserContext : IdentityDbContext<ApplicationUser>
	{
		public UserContext(DbContextOptions<UserContext> options) : base(options)
		{
		}
		public DbSet<ApplicationUser> ApplicationUser { get; set; }
	}
}
