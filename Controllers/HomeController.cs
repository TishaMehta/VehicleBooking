using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Policy;
using VehicleBooking.Areas.LOC_PickupLocation.Models;
using VehicleBooking.Areas.LOC_UserAdmin.Models;
using VehicleBooking.BAL;
using VehicleBooking.DAL;
using VehicleBooking.Models;

namespace VehicleBooking.Controllers
{
    public class HomeController : Controller

    {
        #region global
        public IConfiguration Configuration;
        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private String url = "https://localhost:7099/Count/GetCount/";
        private String Urlbase = "https://localhost:7099/User";

        private HttpClient client = new HttpClient();
        #endregion
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        #region User
        [CheckAccess]
        public IActionResult User()
        {
            return View();
        }
        #endregion

        #region Index

        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region AdminPannel
        [CheckAccess]
        [HttpGet]
        public IActionResult AdminPanel()
        {
            List<CountModel> countModels = new List<CountModel>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                countModels = JsonConvert.DeserializeObject<List<CountModel>>(extDataJason);



            }
            return View(countModels);
        }
        #endregion

        #region Login
        [CheckAccess]
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        #region Registor

        public IActionResult Register()
        {
            return View();
        }
        #endregion

        #region Signup
        [CheckAccess]
        public IActionResult SignUP(RegisterModel register)
        {

            string error = null;
            if (register.FirstName == null)
            {
                error += "FirstName is required";
            }
            if (register.LastName == null)
            {
                error += "<br/>LastName is required";
            }
            if (register.Email == null)
            {
                error += "<br/>Email is required";
            }
            if (register.PhoneNumber == null)
            {
                error += "<br/>PhoneNumber is required";
            }
            if (register.Password == null)
            {
                error += "<br/>Password is required";
            }
            if (error != null)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index");
            }
            MultipartFormDataContent formData = new MultipartFormDataContent();
            formData.Add(new StringContent(register.FirstName), "FirstName");
            formData.Add(new StringContent(register.LastName), "LastName");
            formData.Add(new StringContent(register.Email), "Email");
            formData.Add(new StringContent(register.PhoneNumber), "PhoneNumber");
            formData.Add(new StringContent(register.Password), "Password");
            HttpResponseMessage response = client.PostAsync($"{Urlbase}/Register?FirstName={register.FirstName}&LastName={register.LastName}&Email={register.Email}&PhoneNumber={register.PhoneNumber}&Password={register.Password}", formData).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "User Registered Successfully";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Register");
        }
        #endregion

        #region SignIn
        
        public IActionResult SignIn(LoginModel model)
        {
            string error = null;
            if (model.Name == null)
            {
                error += "UserName or Email required";
            }
            if (model.Password == null)
            {
                error += "<br/>Password is required";
            }

            if (error != null)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index");
            }
            else
            {
                Login_DalBase dal = new Login_DalBase();
                UserAdminModel user = dal.SignIn(model.Name, model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("UserID", user.UserID.ToString());
                    HttpContext.Session.SetString("FirstName", user.FirstName.ToString());
                    HttpContext.Session.SetString("MobileNo", user.LastName.ToString());
                    HttpContext.Session.SetString("Email", user.Email.ToString());
                    HttpContext.Session.SetString("PhoneNumber", user.PhoneNumber.ToString());
                    HttpContext.Session.SetString("Address", user.Address.ToString());
                    HttpContext.Session.SetString("Password", user.Password.ToString());
                    HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

                    if (user.IsAdmin)
                    {
                        return RedirectToAction("AdminPanel", "Home", new { area = "" });
                    }
                    else
                    {
                        return RedirectToAction("User", "Home", new { area = "" });
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Enter correct Information";
                    return View("Index", model);
                }
            }
        }

        #endregion

        #region Logout
        [CheckAccess]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}