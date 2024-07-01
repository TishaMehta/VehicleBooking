using System.ComponentModel.DataAnnotations;

namespace VehicleBooking.Areas.LOC_PickupLocation.Models
{
    public class AdminPickupLocationModel
    {
        public int PickupLocationID { get; set; }
        [Required(ErrorMessage ="The LocationName field is required")]
        public string PickupLocationName { get; set; }
        [Required(ErrorMessage = "The LocationCode field is required")]
        public string PickupLocationCode { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
