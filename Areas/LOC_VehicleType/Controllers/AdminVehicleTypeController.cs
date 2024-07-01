using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VehicleBooking.Areas.LOC_VehicleType.Models;
using VehicleBooking.BAL;

namespace VehicleBooking.Areas.LOC_VehicleType.Controllers
{
    [CheckAccess]

    [Area("LOC_VehicleType")]
    [Route("LOC_VehicleType/[Controller]/[action]")]
    public class AdminVehicleTypeController : Controller
    {
        #region global
        public IConfiguration Configuration;
        public AdminVehicleTypeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private String url = "https://localhost:7099/VehicleType/GetUVehicleType";
        private String baseurl = "https://localhost:7099/VehicleType/DeleteVehicleType?VehicleTypeID=";
        private String puturl = "https://localhost:7099/VehicleType/UpdateVehicleType?VehicleTypeID=";
        private String posturl = "https://localhost:7099/VehicleType/InsertVehicleType";
        private HttpClient client = new HttpClient();

        public string? VehicleTypeName { get; private set; }
        #endregion

        #region AdminVehicleTypeList
        public IActionResult AdminVehicleTypeList(string VehicleTypeName)
        {

            {
                List<AdminVehicleTypeModel> VehicleType = new List<AdminVehicleTypeModel>();
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    String data = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonObject = JsonConvert.DeserializeObject(data);
                    var dataOfObject = jsonObject.data;
                    var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    VehicleType = JsonConvert.DeserializeObject<List<AdminVehicleTypeModel>>(extDataJason);

                    if (!string.IsNullOrEmpty(VehicleTypeName))
                    {
                        VehicleType = VehicleType.Where(vt =>
                            vt.VehicleTypeName.Contains(VehicleTypeName, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }

                }
                return View(VehicleType);
            }
        }
        #endregion

        #region AdminVehicleTypeDelete
        [HttpGet]
        public IActionResult AdminVehicleTypeDelete(int id)
        {
            HttpResponseMessage response = client.DeleteAsync(baseurl + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "user Delete";
            }
            return RedirectToAction("AdminVehicleTypeList");

            //else
            //{
            //    return RedirectToAction("AdminVehicleTypeList");
            //}


        }
        #endregion

        #region Save

        [HttpPost]
        public async Task<IActionResult> Save(AdminVehicleTypeModel adminVehicle)
        {
            try
            {
                MultipartFormDataContent formData = new MultipartFormDataContent();
                formData.Add(new StringContent(adminVehicle.VehicleTypeName), "VehicleTypeName");
                if (adminVehicle.VehicleTypeID == 0)
                {
                    HttpResponseMessage response = await client.PostAsync($"{posturl}", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "VehicleType Inserted";
                        return RedirectToAction("AdminVehicleTypeList");
                    }
                }
                else
                {
                    HttpResponseMessage response = await client.PutAsync($"{puturl}" + adminVehicle.VehicleTypeID, formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "VehicleType Updated";
                        return RedirectToAction("AdminVehicleTypeList");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["Eerror"] = "An error" + e.Message;
            }
            return RedirectToAction("AdminVehicleTypeList");
        }
#endregion

        #region AddEdit
        [HttpGet]

        public IActionResult Edit(int id)
        {
            AdminVehicleTypeModel adminVehicle = new AdminVehicleTypeModel();

            HttpResponseMessage response = client.GetAsync($"https://localhost:7099/VehicleType/GetBooking/+{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                adminVehicle = JsonConvert.DeserializeObject<AdminVehicleTypeModel>(extDataJason);
            }
           
            return View("VehicleTypeAddEdit", adminVehicle);
        }
        #endregion

    }
}







