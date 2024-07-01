using System.ComponentModel.DataAnnotations;

namespace VehicleBooking.Areas.Fleet.Models
{
    public class FleetModel
    {
        public int VehicleId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string VehicleImage { get; set; }
        [Required]
        public string VehicleName { get; set; }
        [Required]
        public int VehicleTypeID { get; set; }
        [Required]
        public string VehicleTypeName { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of passengers must be a positive integer.")]

        public int NumberOfPassenger { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Charge per KM must be a positive decimal.")]

        public decimal ChargePerKM { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }

    }

    public class AdminBookingModels
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

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }

}
