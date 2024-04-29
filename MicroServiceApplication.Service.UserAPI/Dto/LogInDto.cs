using System.ComponentModel.DataAnnotations;

namespace MicroServiceApplication.Service.UserAPI.Dto
{
	public class LogInDto
	{
        [EmailAddress]
        public string Email { get; set; }
		public string Password { get; set; }
	}
}
