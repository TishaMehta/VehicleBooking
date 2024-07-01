using System.ComponentModel.DataAnnotations;

namespace VehicleBooking.Areas.BookNow.Models
{
    public class BookNowModels
    {
        public int BookingID { get; set; }
        [Required(ErrorMessage = "User Name is required.")]
        public int UserID { get; set; }
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Vehicle Name is required.")]

        public int VehicleID { get; set; }
        public string? VehicleName { get; set; }
        [Required(ErrorMessage = "PickupLocation Name is required.")]

        public int PickupLocationID { get; set; }

        public string? PickupLocationName { get; set; }
        [Required(ErrorMessage = "DropOffLocation Name is required.")]

        public int DropOffLocationID { get; set; }

        public string? DropOffLocationName { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DropOffDate { get; set; }

        public string? BookingStatus { get; set; }
        public decimal? TotalCharge { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }

    public class UserDropDown
    {
        public int UserID { get; set; }
        public string? FirstName { get; set; }

    }
    public class VehicleDropDown
    {
        public int VehicleID { get; set; }
        public string? VehicleName { get; set; }

    }
    public class PickupLocationDropDown
    {
        public int PickupLocationID { get; set; }
        public string? PickupLocationName { get; set; }

    }
    public class DropoffLocationDropDown
    {
        public int DropOffLocationID { get; set; }
        public string? DropOffLocationName { get; set; }

    }

}
