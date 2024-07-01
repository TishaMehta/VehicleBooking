using System.ComponentModel.DataAnnotations;

namespace VehicleBooking.Models
{
	public class LoginModel
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;
	}
}
