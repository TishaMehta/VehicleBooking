using System.ComponentModel.DataAnnotations;

namespace VehicleBooking.Areas.LOC_Payment.Models
{
    public class AdminPaymentModel
    {
        public int PaymentID { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Booking ID must be a positive integer.")]

        public int BookingID { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive decimal.")]

        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment date is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}")]

        public DateTime PaymentDate { get; set; }
        [Required]
        public string? PaymentStatus { get; set; }
        [Required]
        public string? PaymentMethod { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }
    public class BookingDropdown
    {
        public int BookingID { get; set; }
    }
}
