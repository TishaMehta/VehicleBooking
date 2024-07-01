using System.ComponentModel.DataAnnotations;

namespace VehicleBooking.Areas.LOC_Vehicles.Models
{
    public class AdminVehiclesModel
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
        [Range(0.01, double.MaxValue, ErrorMessage = "Charge per day must be a positive decimal.")]

        public decimal ChargePerKM { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }

    }
    public class VehicleTypeDropDown
    {
        public int VehicleTypeID { get; set; }
        public string? VehicleTypeName { get; set; }
    }
}
