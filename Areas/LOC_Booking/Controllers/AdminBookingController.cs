using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VehicleBooking.Areas.LOC_Booking.Models;
using VehicleBooking.Areas.LOC_DropoffLocation.Models;
using VehicleBooking.Areas.LOC_Vehicles.Models;
using VehicleBooking.BAL;

namespace VehicleBooking.Areas.LOC_Booking.Controllers
{
    [CheckAccess]

    [Area("LOC_Booking")]
    [Route("LOC_Booking/[Controller]/[action]")]

    public class AdminBookingController : Controller
    {
        #region global
        public IConfiguration Configuration;
        public AdminBookingController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private String url = "https://localhost:7099/Booking/GetBooking";
        private String baseurl = "https://localhost:7099/Booking/DeleteBooking?BookingID=";
        private String posturl = "https://localhost:7099/Booking/InsertBooking";
        private String puturl = "https://localhost:7099/Booking/UpdateBooking?BookingID=";
        private String Url = "https://localhost:7099/User";
        private HttpClient client = new HttpClient();
        #endregion

        
        #region BookingList Page
        public IActionResult AdminBookingList(string FirstName,string VehicleName,string PickupLocationName,string DropoffLocationName)
        {

            {
                List<AdminBookingModels> booking = new List<AdminBookingModels>();
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    String data = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonObject = JsonConvert.DeserializeObject(data);
                    var dataOfObject = jsonObject.data;
                    var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    booking = JsonConvert.DeserializeObject<List<AdminBookingModels>>(extDataJason);
                    if (!string.IsNullOrEmpty(FirstName))
                    {
                        booking = booking.Where(vt =>
                            vt.FirstName.Contains(FirstName, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }
                    if (!string.IsNullOrEmpty(VehicleName))
                    {
                        booking = booking.Where(vt =>
                            vt.VehicleName.Contains(VehicleName, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }

                    if (!string.IsNullOrEmpty(PickupLocationName))
                    {
                        booking = booking.Where(vt =>
                            vt.PickupLocationName.Contains(PickupLocationName, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }

                    if (!string.IsNullOrEmpty(DropoffLocationName))
                    {
                        booking = booking.Where(vt =>
                            vt.DropOffLocationName.Contains(DropoffLocationName, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }
                }
                return View(booking);
            }
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult AdminBookingDelete(int id)
        {
            HttpResponseMessage response = client.DeleteAsync(baseurl + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Booking Delete";
                return RedirectToAction("AdminBookingList");
            }
            else
            {
                return RedirectToAction("AdminBookingList");
            }


        }
        #endregion

        #region Save

        [HttpPost]
        public async Task<IActionResult> Save(AdminBookingModels adminBooking)
        {
            try
            {
                MultipartFormDataContent formData = new MultipartFormDataContent();
                formData.Add(new StringContent(adminBooking.UserID.ToString()), "UserID");
                formData.Add(new StringContent(adminBooking.VehicleID.ToString()), "VehicleID");
                formData.Add(new StringContent(adminBooking.PickupLocationID.ToString()), "PickupLocationID");
                formData.Add(new StringContent(adminBooking.DropOffLocationID.ToString()), "DropOffLocationID");
                formData.Add(new StringContent(adminBooking.PickupDate.ToString()), "PickupDate");
                formData.Add(new StringContent(adminBooking.DropOffDate.ToString()), "DropOffDate");
                //formData.Add(new StringContent(adminBooking.BookingStatus.ToString()), "BookingStatus");
                //formData.Add(new StringContent(adminBooking.TotalCharge.ToString()), "TotalCharge");




                if (adminBooking.BookingID == 0)
                {
                    HttpResponseMessage response = await client.PostAsync($"{posturl}", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Booking Inserted";
                        return RedirectToAction("AdminBookingList");
                    }
                }
                else
                {
                    HttpResponseMessage response = await client.PutAsync($"{puturl}" + adminBooking.BookingID, formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Booking Updated";
                        return RedirectToAction("AdminBookingList");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["Eerror"] = "An error" + e.Message;
            }
            return RedirectToAction("AdminBookingList");
        }
        #endregion

        #region AddEdit
        [HttpGet]

        public IActionResult Edit(int id)
        {
            //DropDownUser();
            DropDownVehicle();
            DropDownPickupLocation();
            DropDownDropoffLocation();
            AdminBookingModels adminBooking = new AdminBookingModels();

            HttpResponseMessage response = client.GetAsync($"https://localhost:7099/Booking/GetBooking/+{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                adminBooking = JsonConvert.DeserializeObject<AdminBookingModels>(extDataJason);
            }
           
            return View("AdminBookingAddEdit", adminBooking);
        }
        #endregion

        #region DropDown User
        public void DropDownUser()
        {

            List<UserDropDown> userDrops = new List<UserDropDown>();
            HttpResponseMessage response = client.GetAsync("https://localhost:7099/User/GetUSER/").Result;
            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                userDrops = JsonConvert.DeserializeObject<List<UserDropDown>>(extDataJason);

            }
            ViewBag.userDrops = userDrops;
        }
        #endregion

        #region DropDownVehicle
        public void DropDownVehicle()
        {

            List<VehicleDropDown> vehicleDrops = new List<VehicleDropDown>();
            HttpResponseMessage response = client.GetAsync("https://localhost:7099/Vehicles/Get/").Result;
            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                vehicleDrops = JsonConvert.DeserializeObject<List<VehicleDropDown>>(extDataJason);

            }
            ViewBag.vehicleDrops = vehicleDrops;
        }
        #endregion

        #region DropDownPickupLocation
        public void DropDownPickupLocation()
        {

            List<PickupLocationDropDown> pickupLocationDrops = new List<PickupLocationDropDown>();
            HttpResponseMessage response = client.GetAsync("https://localhost:7099/PickupLocation/GetPickupLocation/").Result;
            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                pickupLocationDrops = JsonConvert.DeserializeObject<List<PickupLocationDropDown>>(extDataJason);

            }
            ViewBag.pickupLocationDrops = pickupLocationDrops;
        }
        #endregion

        #region DropDown DropDownDropoffLocation
        public void DropDownDropoffLocation()
        {

            List<DropoffLocationDropDown> dropoffLocationDrops = new List<DropoffLocationDropDown>();
            HttpResponseMessage response = client.GetAsync("https://localhost:7099/DropoffLocation/GetDropoffLocation/").Result;
            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                dropoffLocationDrops = JsonConvert.DeserializeObject<List<DropoffLocationDropDown>>(extDataJason);

            }
            ViewBag.dropoffLocationDrops = dropoffLocationDrops;
        }
        #endregion

    }
}
