using System.ComponentModel.DataAnnotations;

namespace VehicleBooking.Areas.LOC_VehicleType.Models
{
    public class AdminVehicleTypeModel
    {
        public int VehicleTypeID { get; set; }
        [Required]
        public string VehicleTypeName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
