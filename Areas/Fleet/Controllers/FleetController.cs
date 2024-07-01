using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VehicleBooking.Areas.Fleet.Models;
using VehicleBooking.Areas.LOC_Vehicles.Models;
using VehicleBooking.BAL;

namespace VehicleBooking.Areas.Fleet.Controllers
{
    [CheckAccess]

    [Area("Fleet")]
    [Route("Fleet/[Controller]/[action]")]

    public class FleetController : Controller
    {
      
        #region global
        public IConfiguration Configuration;
        public FleetController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private String url = "https://localhost:7099/Vehicles/Get";
        private String baseurl = "https://localhost:7099/Vehicles/Delete?VehicleID=";
        private String posturl = "https://localhost:7099/Vehicles/Insert";
        private String puturl = "https://localhost:7099/Vehicles/Update?VehicleID=";
        private String posturl1 = "https://localhost:7099/Booking/InsertBooking";

        private HttpClient client = new HttpClient();
        #endregion

        #region FleetList
        public IActionResult FleetList(string VehicleTypeName)
        {
            {
                List<FleetModel> vehicles = new List<FleetModel>();
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    String data = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonObject = JsonConvert.DeserializeObject(data);
                    var dataOfObject = jsonObject.data;
                    var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                    vehicles = JsonConvert.DeserializeObject<List<FleetModel>>(extDataJason);
                    if (!string.IsNullOrEmpty(VehicleTypeName))
                    {
                        vehicles = vehicles.Where(vt =>
                            vt.VehicleTypeName.Contains(VehicleTypeName, StringComparison.OrdinalIgnoreCase)
                        ).ToList();
                    }




                }
                return View(vehicles);
            }
        }
        #endregion

        #region FleetID
        [HttpGet]

        public IActionResult FleetID(int id)
        {
            FleetModel fleetModel = new FleetModel();

            HttpResponseMessage response = client.GetAsync($"https://localhost:7099/Vehicles/Get/+{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                fleetModel = JsonConvert.DeserializeObject<FleetModel>(extDataJason);
            }

            return View(fleetModel);
        }
        #endregion

      

    }
}
