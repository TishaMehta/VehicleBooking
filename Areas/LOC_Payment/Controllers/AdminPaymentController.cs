using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VehicleBooking.Areas.LOC_DropoffLocation.Models;
using VehicleBooking.Areas.LOC_Payment.Models;
using VehicleBooking.Areas.LOC_PickupLocation.Models;
using VehicleBooking.Areas.LOC_UserAdmin.Models;
using VehicleBooking.Areas.LOC_Vehicles.Models;
using VehicleBooking.BAL;

namespace VehicleBooking.Areas.LOC_Payment.Controllers
{
    [CheckAccess]

    [Area("LOC_Payment")]
    [Route("LOC_Payment/[Controller]/[action]")]

    public class AdminPaymentController : Controller
    {
        #region global
        public IConfiguration Configuration;
        public AdminPaymentController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private String url = "https://localhost:7099/Payment/GetPayment";
        private String baseurl = "https://localhost:7099/Payment/DeletePayment?PaymentID=";
        private String posturl = "https://localhost:7099/Payment/InsertPayment";
        private String puturl = "https://localhost:7099/Payment/UpdatePayment?PaymentID=";
        private HttpClient client = new HttpClient();
        #endregion

        #region AdminPaymentList
        [HttpGet]
        public IActionResult AdminPaymentList(string PaymentMethod)
        {
            List<AdminPaymentModel> payment = new List<AdminPaymentModel>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                payment = JsonConvert.DeserializeObject<List<AdminPaymentModel>>(extDataJason);
              
                if (!string.IsNullOrEmpty(PaymentMethod))
                {
                    payment = payment.Where(vt =>
                        vt.PaymentMethod.Contains(PaymentMethod, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

            
            }
            return View(payment);
        }
        #endregion

        #region AdminPaymentDelete
        [HttpGet]
        public IActionResult AdminPaymentDelete(int id)
        {
            HttpResponseMessage response = client.DeleteAsync(baseurl + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "payment Delete";
                return RedirectToAction("AdminPaymentList");
            }
            else
            {
                return RedirectToAction("AdminPaymentList");
            }


        }
        #endregion

        #region Save

        [HttpPost]
        public async Task<IActionResult> Save(AdminPaymentModel adminPayment)
        {
            try
            {
                MultipartFormDataContent formData = new MultipartFormDataContent();
                formData.Add(new StringContent(adminPayment.BookingID.ToString()), "BookingID");
                formData.Add(new StringContent(adminPayment.Amount.ToString()), "Amount");
                formData.Add(new StringContent(adminPayment.PaymentDate.ToString()), "PaymentDate");
                formData.Add(new StringContent(adminPayment.PaymentStatus), "PaymentStatus");
                formData.Add(new StringContent(adminPayment.PaymentMethod), "PaymentMethod");


                if (adminPayment.PaymentID == 0)
                {
                    HttpResponseMessage response = await client.PostAsync($"{posturl}", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Payment Inserted";
                        return RedirectToAction("AdminPaymentList");
                    }
                }
                else
                {
                    HttpResponseMessage response = await client.PutAsync($"{puturl}" + adminPayment.PaymentID, formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Payment Updated";
                        return RedirectToAction("AdminPaymentList");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["Eerror"] = "An error" + e.Message;
            }
            return RedirectToAction("AdminPaymentList");
        }
        #endregion

        #region AddEdit
        [HttpGet]

        public IActionResult Edit(int id)
        {
            DropDownBooking();
            AdminPaymentModel adminPayment = new AdminPaymentModel();

            HttpResponseMessage response = client.GetAsync($"https://localhost:7099/Payment/GetPayment/+{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                adminPayment = JsonConvert.DeserializeObject<AdminPaymentModel>(extDataJason);
            }
            return View("AdminPaymentAddEdit", adminPayment);
        }
        #endregion

        #region DropDown Booking
        public void DropDownBooking()
        {

            List<BookingDropdown> bookingDropdowns = new List<BookingDropdown>();
            HttpResponseMessage response = client.GetAsync("https://localhost:7099/Booking/GetBooking/").Result;
            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                bookingDropdowns = JsonConvert.DeserializeObject<List<BookingDropdown>>(extDataJason);

            }
            ViewBag.school = bookingDropdowns;
        }
        #endregion

    }
}
