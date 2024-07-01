using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VehicleBooking.Areas.LOC_Payment.Models;
using VehicleBooking.Areas.LOC_PickupLocation.Models;
using VehicleBooking.Areas.LOC_UserAdmin.Models;
using VehicleBooking.BAL;

namespace VehicleBooking.Areas.LOC_PickupLocation.Controllers
{
    [CheckAccess]

    [Area("LOC_PickupLocation")]
    [Route("LOC_PickupLocation/[Controller]/[action]")]

    public class AdminPickupLocationController : Controller
    {
        #region global
        public IConfiguration Configuration;
        public AdminPickupLocationController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private String url = "https://localhost:7099/PickupLocation/GetPickupLocation/";
        private String baseurl = "https://localhost:7099/PickupLocation/DeletePickupLocation?PickupLocatioID=";
        private String posturl = "https://localhost:7099/PickupLocation/InsertLocation";
        private String puturl = "https://localhost:7099/PickupLocation/UpdatePickupLocation?PickupLocationID=";
        private HttpClient client = new HttpClient();
        #endregion

        #region ExportExl
        public List<AdminPickupLocationModel> Index1()
        {
            List<AdminPickupLocationModel> pickuplocation = new List<AdminPickupLocationModel>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                pickuplocation = JsonConvert.DeserializeObject<List<AdminPickupLocationModel>>(extDataJason);

            }
            return pickuplocation;
        }

        public IActionResult ExportData()
        {
            List<AdminPickupLocationModel> pickuplocation = Index1();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");

                worksheet.Cell(1, 1).Value = "PickupLocation ID";
                worksheet.Cell(1, 2).Value = "PickupLocation Name";
                worksheet.Cell(1, 3).Value = "PickupLocation Code";


                int row = 2;

                foreach (var r in pickuplocation)
                {
                    worksheet.Cell(row, 1).Value = r.PickupLocationID;
                    worksheet.Cell(row, 2).Value = r.PickupLocationName;
                    worksheet.Cell(row, 3).Value = r.PickupLocationCode;


                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "exported_data.xlsx");
                }
            }
        }
        #endregion

        #region AdminPickupLocationList
        [HttpGet]
        public IActionResult AdminPickupLocationList(string pickupLocationCodeFilter, string pickupLocationNameFilter)
        {
            List<AdminPickupLocationModel> pickupLocationModels = new List<AdminPickupLocationModel>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                pickupLocationModels = JsonConvert.DeserializeObject<List<AdminPickupLocationModel>>(extDataJason);

                if (!string.IsNullOrEmpty(pickupLocationCodeFilter))
                {
                    pickupLocationModels = pickupLocationModels
                        .Where(pl => pl.PickupLocationCode.Contains(pickupLocationCodeFilter))
                        .ToList();
                }

                if (!string.IsNullOrEmpty(pickupLocationNameFilter))
                {
                    pickupLocationModels = pickupLocationModels
                        .Where(pl => pl.PickupLocationName.Contains(pickupLocationNameFilter))
                        .ToList();
                }

            }
            return View(pickupLocationModels);
        }
        #endregion

        #region AdminPickupLocationDelete
        [HttpGet]
        public IActionResult AdminPickupLocationDelete(int id)
        {
            HttpResponseMessage response = client.DeleteAsync(baseurl + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "PickupLocation Delete";
                return RedirectToAction("AdminPickupLocationList");
            }
            else
            {
                return RedirectToAction("AdminPickupLocationList");
            }


        }
        #endregion

        #region Save

        [HttpPost]
        public async Task<IActionResult> Save(AdminPickupLocationModel adminPickup)
        {
            try
            {
                MultipartFormDataContent formData = new MultipartFormDataContent();
                formData.Add(new StringContent(adminPickup.PickupLocationName), "PickupLocationName");
                formData.Add(new StringContent(adminPickup.PickupLocationCode), "PickupLocationCode");
               
                if (adminPickup.PickupLocationID == 0)
                {
                    HttpResponseMessage response = await client.PostAsync($"{posturl}", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "PickupLocation Inserted";
                        return RedirectToAction("AdminPickupLocationList");
                    }
                }
                else
                {
                    HttpResponseMessage response = await client.PutAsync($"{puturl}" + adminPickup.PickupLocationID, formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "PickupLocation Updated";
                        return RedirectToAction("AdminPickupLocationList");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["Eerror"] = "An error" + e.Message;
            }
            return RedirectToAction("AdminPickupLocationList");
        }
        #endregion

        #region AddEdit
        [HttpGet]

        public IActionResult Edit(int id)
        {
            AdminPickupLocationModel adminPickup = new AdminPickupLocationModel();

            HttpResponseMessage response = client.GetAsync($"https://localhost:7099/PickupLocation/GetPickupLocation/+{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                adminPickup = JsonConvert.DeserializeObject<AdminPickupLocationModel>(extDataJason);
            }
            
        return View("AdminPickupLocationAddEdit", adminPickup);
        }
        #endregion

    }
}
