using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VehicleBooking.Areas.LOC_DropoffLocation.Models;
using VehicleBooking.Areas.LOC_Payment.Models;
using VehicleBooking.BAL;

namespace VehicleBooking.Areas.LOC_DropoffLocation.Controllers
{
    [CheckAccess]

    [Area("LOC_DropoffLocation")]
    [Route("LOC_DropoffLocation/[Controller]/[action]")]

    public class AdminDropoffLocationController : Controller
    {
        #region global
        public IConfiguration Configuration;
        public AdminDropoffLocationController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private String url = "https://localhost:7099/DropoffLocation/GetDropoffLocation";
        private String baseurl = "https://localhost:7099/DropoffLocation/DeleteDropoffLocation?DropoffLocationID=";
        private String posturl = "https://localhost:7099/DropoffLocation/InsertDropoffLocation";
        private String puturl = "https://localhost:7099/DropoffLocation/UpdateDropoffLocation?DropffLocationID=";
        private HttpClient client = new HttpClient();
        #endregion

        #region List
        public IActionResult AdminDropoffLocationList(string DropoffLocationName, string DropoffLocationCode)
        {
            List<AdminDropoffLocationModel> adminDropoffLocationModels = new List<AdminDropoffLocationModel>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                adminDropoffLocationModels = JsonConvert.DeserializeObject<List<AdminDropoffLocationModel>>(extDataJason);
              
                if (!string.IsNullOrEmpty(DropoffLocationName))
                {
                    adminDropoffLocationModels = adminDropoffLocationModels.Where(vt =>
                        vt.DropoffLocationName.Contains(DropoffLocationName, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                if (!string.IsNullOrEmpty(DropoffLocationCode))
                {
                    adminDropoffLocationModels = adminDropoffLocationModels.Where(vt =>
                        vt.DropoffLocationCode.Contains(DropoffLocationCode, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }




            }
            return View(adminDropoffLocationModels);
        }
        #endregion

        #region Delete
        public IActionResult AdminDropoffLocationDelete(int id)
        {
            HttpResponseMessage response = client.DeleteAsync(baseurl + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "DropoffLocation Delete";
                return RedirectToAction("AdminDropoffLocationList");
            }
            else
            {
                return RedirectToAction("AdminDropoffLocationList");
            }


        }
        #endregion

        #region Save

        [HttpPost]
        public async Task<IActionResult> Save(AdminDropoffLocationModel adminDropoff)
        {
            try
            {
                MultipartFormDataContent formData = new MultipartFormDataContent();
                formData.Add(new StringContent(adminDropoff.DropoffLocationName), "DropoffLocationName");
                formData.Add(new StringContent(adminDropoff.DropoffLocationCode), "DropoffLocationCode");
               

                if (adminDropoff.DropoffLocationID == 0)
                {
                    HttpResponseMessage response = await client.PostAsync($"{posturl}", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "DropoffLocation Inserted";
                        return RedirectToAction("AdminDropoffLocationList");
                    }
                }
                else
                {
                    HttpResponseMessage response = await client.PutAsync($"{puturl}" + adminDropoff.DropoffLocationID, formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "DropoffLocation Updated";
                        return RedirectToAction("AdminDropoffLocationList");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["Eerror"] = "An error" + e.Message;
            }
            return RedirectToAction("AdminDropoffLocationList");
        }
        #endregion

        #region AddEdit
        [HttpGet]

        public IActionResult Edit(int id)
        {
            AdminDropoffLocationModel adminDropoff = new AdminDropoffLocationModel();

            HttpResponseMessage response = client.GetAsync($"https://localhost:7099/DropoffLocation/GetDropoffLocatin//+{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                adminDropoff = JsonConvert.DeserializeObject<AdminDropoffLocationModel>(extDataJason);
            }
           
            return View("AdminDropoffLocationAddEdit", adminDropoff);
        }
        #endregion


    }
}
