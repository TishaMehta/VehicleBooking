using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Text;
using VehicleBooking.Areas.LOC_DropoffLocation.Models;
using VehicleBooking.Areas.LOC_UserAdmin.Models;
using VehicleBooking.Areas.LOC_Vehicles.Models;
using VehicleBooking.BAL;

namespace VehicleBooking.Areas.LOC_UserAdmin.Controllers
{
    [CheckAccess]
    [Area("LOC_UserAdmin")]
    [Route("LOC_UserAdmin/[Controller]/[action]")]
    public class UserAdminController : Controller
    {
        #region global
        public IConfiguration Configuration;
        public UserAdminController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private String url = "https://localhost:7099/User/GetUSER";
        private String baseurl = "https://localhost:7099/User/DeleteUser/Delete1?UserID=";
        private String posturl = "https://localhost:7099/User/InsertUser";
        private String puturl = "https://localhost:7099/User/Update?UserID=";
        private HttpClient client = new HttpClient();
        #endregion

        #region AdminUserList
        [HttpGet]
        public IActionResult AdminUserList(String FirstName,string LastName,String Email,string PhoneNumber,string Address)
        {
            List<UserAdminModel> User = new List<UserAdminModel>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                String data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                User = JsonConvert.DeserializeObject<List<UserAdminModel>>(extDataJason);
                if (!string.IsNullOrEmpty(FirstName))
                {
                    User = User.Where(vt =>
                        vt.FirstName.Contains(FirstName, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }
                if (!string.IsNullOrEmpty(LastName))
                {
                    User = User.Where(vt =>
                        vt.LastName.Contains(LastName, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }
                if (!string.IsNullOrEmpty(Email))
                {
                    User = User.Where(vt =>
                        vt.Email.Contains(Email, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }
                if (!string.IsNullOrEmpty(PhoneNumber))
                {
                    User = User.Where(vt =>
                        vt.PhoneNumber.Contains(PhoneNumber, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }
                if (!string.IsNullOrEmpty(Address))
                {
                    User = User.Where(vt =>
                        vt.Address.Contains(Address, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

            }
            return View(User);
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult AdminUserID(int id)
        {
            UserAdminModel userAdminModel = new UserAdminModel();

            HttpResponseMessage response = client.GetAsync(url + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                userAdminModel = JsonConvert.DeserializeObject<UserAdminModel>(extDataJason);
            }
            else
            {
                return RedirectToAction("Error");
            }

            return View(userAdminModel);
        }
        #endregion

        #region AdminUserDelete
        [HttpGet]
        public IActionResult AdminUserDelete(int id)
        {
            HttpResponseMessage response = client.DeleteAsync(baseurl + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "user Delete";
                return RedirectToAction("AdminUserList");
            }
            else
            {
                return RedirectToAction("AdminUserList");
            }
            

        }
        #endregion

        #region Save

        [HttpPost]
        public async Task<IActionResult> Save(UserAdminModel userAdmin)
        {
            try
            {
                MultipartFormDataContent formData = new MultipartFormDataContent();
                formData.Add(new StringContent(userAdmin.FirstName), "FirstName");
                formData.Add(new StringContent(userAdmin.LastName), "LastName");
                formData.Add(new StringContent(userAdmin.Email), "Email");
                formData.Add(new StringContent(userAdmin.PhoneNumber), "PhoneNumber");
                formData.Add(new StringContent(userAdmin.Password), "Password");
                formData.Add(new StringContent(userAdmin.Address), "Address");
                formData.Add(new StringContent(userAdmin.IsAdmin.ToString()), "IsAdmin");

                if (userAdmin.UserID == 0)
                {
                    HttpResponseMessage response = await client.PostAsync($"{posturl}", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "User Inserted";
                        return RedirectToAction("AdminUserList");
                    }
                }
                else
                {
                    HttpResponseMessage response = await client.PutAsync($"{puturl}" + userAdmin.UserID, formData);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "User Updated";
                        return RedirectToAction("AdminUserList");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["Eerror"] = "An error" + e.Message;
            }
            return RedirectToAction("AdminUserList");
        }
        #endregion

        #region AddEdit
        [HttpGet]

        public IActionResult Edit(int id)
        {
            UserAdminModel userAdmin = new UserAdminModel();

            HttpResponseMessage response = client.GetAsync($"https://localhost:7099/User/GetUser/+{id}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject(data);
                var dataOfObject = jsonObject.data;
                var extDataJason = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                userAdmin = JsonConvert.DeserializeObject<UserAdminModel>(extDataJason);
                //return View("AdminUserAddEdit", userAdmin);
            }
            //else
            //{
            //    return RedirectToAction("Error");
            //}
            return View("AdminUserAddEdit", userAdmin);
        }
        #endregion


    }

}
