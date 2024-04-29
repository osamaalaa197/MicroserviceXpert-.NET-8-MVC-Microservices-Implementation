using System.ComponentModel.DataAnnotations;

namespace MicroServiceApplication.Service.UserAPI.Dto
{
	public class RegisterDto
	{
        [EmailAddress]
        public string Email { get; set; }
		public string Password { get; set; }
		public string PhoneNumber { get; set; }
		public string Name { get; set; }
        public string? Role { get; set; }

    }
}
