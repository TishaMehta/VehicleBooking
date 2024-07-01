using System.ComponentModel.DataAnnotations;

namespace VehicleBooking.Areas.LOC_UserAdmin.Models
{
    public class UserAdminModel
    {
        public int UserID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [MaxLength (10)]
        [MinLength(5)]
        public string Password { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
    public class UserLoginModel
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
    }

}
