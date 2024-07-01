using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using VehicleBooking.Areas.LOC_Vehicles.Models;
using VehicleBooking.Areas.LOC_VehicleType.Models;
using VehicleBooking.BAL;

namespace VehicleBooking.Areas.LOC_Vehicles.Controllers
{
    [CheckAccess]

    [Area("LOC_Vehicles")]
    [Route("LOC_Vehicles/[Controller]/[action]")]
    public class AdminVehiclesController : Controller
    {
        #region global
        public IConfiguration Configuration;
        public AdminVehiclesController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private String url = "https://localhost:7099/Vehicles/Get";
        private String baseurl = "https://localhost:7099/Vehicles/Delete?VehicleID=";
        private String posturl = "https://localhost:7099/Vehicles/Insert";
        private String puturl = "https://localhost:7099/Vehicles/Update?VehicleID=";
        private HttpClient client = new HttpClient();
        #endregion

        #region AdminVehiclesList
        public IActionResult AdminVehiclesList(string VehicleName, string VehicleTypeName)
        {

            {
                List<AdminVehiclesModel> vehicles = new List<AdminVehiclesModel>();
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    String data = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonObject = JsonConvert.DeserializeObject(data);
                    var dataOfObject = jsonObject.data;
                    var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    vehicles = JsonConvert.DeserializeObject<List<AdminVehiclesModel>>(extDataJason);

                    if (!string.IsNullOrEmpty(VehicleName))
                    {
                        vehicles = vehicles.Where(vt =>
                            vt.VehicleName.Contains(VehicleName, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }

                    if (!string.IsNullOrEmpty(VehicleTypeName))
                    {
                        vehicles = vehicles.Where(vt =>
                            vt.VehicleTypeName.Contains(VehicleTypeName, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }


                    //var distinctVehicleTypeNames = vehicles.Select(v => v.VehicleTypeName).Distinct().ToList();

                    //ViewBag.VehicleTypeNames = new SelectList(distinctVehicleTypeNames);

                }
                return View(vehicles);
            }
        }
        #endregion

        #region AdminVehiclesID
        [HttpGet]

        public IActionResult AdminVehiclesID(int id)
        {
            AdminVehiclesModel adminVehicle = new AdminVehiclesModel();

            HttpResponseMessage response = client.GetAsync($"https://localhost:7099/Vehicles/Get/+{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                adminVehicle = JsonConvert.DeserializeObject<AdminVehiclesModel>(extDataJason);
            }

            return View(adminVehicle);
        }
        #endregion

        #region Delete
        public IActionResult AdminVehicleDelete(int id)
        {
            HttpResponseMessage response = client.DeleteAsync(baseurl + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "user Delete";
                return RedirectToAction("AdminVehiclesList");
            }
            else
            {
                return RedirectToAction("AdminVehiclesList");
            }


        }
        #endregion

        #region Save

        [HttpPost]
        public async Task<IActionResult> Save(AdminVehiclesModel adminVehicles)
        {
            try
            {
                //if (adminVehicles.ImageFile != null && adminVehicles.ImageFile.Length > 0)
                //{
                //    var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", "Vehicle", adminVehicles.VehicleName + "." + adminVehicles.ImageFile.ContentType.Split('/')[1]);
                //    using (FileStream stream = new FileStream(path, FileMode.Create))
                //    {
                //        adminVehicles.ImageFile.CopyTo(stream);
                //    }
                //    adminVehicles.VehicleImage = adminVehicles.VehicleName + "." + adminVehicles.ImageFile.ContentType.Split('/')[1];
                //}



                MultipartFormDataContent formData = new MultipartFormDataContent();
                formData.Add(new StringContent(adminVehicles.VehicleName), "VehicleName");
                formData.Add(new StringContent(adminVehicles.VehicleTypeID.ToString()), "VehicleTypeID");
                formData.Add(new StringContent(adminVehicles.RegistrationNumber), "RegistrationNumber");
                formData.Add(new StringContent(adminVehicles.NumberOfPassenger.ToString()), "NumberOfPassenger");
                formData.Add(new StringContent(adminVehicles.Status), "Status");
                formData.Add(new StringContent(adminVehicles.ChargePerKM.ToString()), "ChargePerKM");

                if (adminVehicles.VehicleId == 0)
                {
                    HttpResponseMessage response = await client.PostAsync($"{posturl}", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Vehicle Inserted";
                        return RedirectToAction("AdminVehiclesList");
                    }
                }
                else
                {
                    HttpResponseMessage response = await client.PutAsync($"{puturl}" + adminVehicles.VehicleId, formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Vehicle Updated";
                        return RedirectToAction("AdminVehiclesList");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["Eerror"] = "An error" + e.Message;
            }
            return RedirectToAction("AdminVehiclesList");
        }
        #endregion
       
        #region AddEdit
        [HttpGet]

        public IActionResult Edit(int id)
        {
            DropDownType();
            AdminVehiclesModel adminVehicles = new AdminVehiclesModel();

            HttpResponseMessage response = client.GetAsync($"https://localhost:7099/Vehicles/Get/+{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                adminVehicles = JsonConvert.DeserializeObject<AdminVehiclesModel>(extDataJason);
            }
            return View("AdminVehiclesAddEdit", adminVehicles);
        }
        #endregion

        #region DropDown VehicleType
        public void DropDownType()
        {

            List<VehicleTypeDropDown> vehicleTypeDropDowns = new List<VehicleTypeDropDown>();
            HttpResponseMessage response = client.GetAsync("https://localhost:7099/VehicleType/GetUVehicleType/").Result;
            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                vehicleTypeDropDowns = JsonConvert.DeserializeObject<List<VehicleTypeDropDown>>(extDataJason);

            }
            ViewBag.school = vehicleTypeDropDowns;
        }
        #endregion
    }
}
