namespace VehicleBooking.Areas.LOC_DropoffLocation.Models
{
    public class AdminDropoffLocationModel
    {
        public int DropoffLocationID { get; set; }

        public string? DropoffLocationName { get; set; }
        public string? DropoffLocationCode { get; set; }
        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }
}
