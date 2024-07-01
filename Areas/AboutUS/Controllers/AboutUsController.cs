using Microsoft.AspNetCore.Mvc;
using VehicleBooking.BAL;

namespace VehicleBooking.Areas.AboutUS.Controllers
{
    [CheckAccess]

    [Area("AboutUS")]
    [Route("AboutUS/[Controller]/[action]")]

    public class AboutUsController : Controller
    {
        public IActionResult AboutUS()
        {
            return View();
        }
    }
}
