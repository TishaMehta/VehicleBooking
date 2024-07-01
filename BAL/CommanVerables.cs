namespace VehicleBooking.BAL
{
    public class CommanVerables
    {
        //Provides access to the current 

        //Microsoft.AspNetCore.Http.IHttpContextAccessor.HttpContext

        private static IHttpContextAccessor _httpContextAccessor;
        static CommanVerables()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }
        public static bool IsAdmin()
        {
            if (_httpContextAccessor.HttpContext.Session.GetString("IsAdmin") != null)
            {
                if (_httpContextAccessor.HttpContext.Session.GetString("IsAdmin") == "True")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static int? UserID()
        {
            //Initialize the UserID with null
            int? UserID = null;
            //check if session contains specified key?
            //if it contains then return the value contained by the key.
            if (_httpContextAccessor.HttpContext.Session.GetString("UserID") != null)
            {
                UserID =
               Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("UserID").ToString());
            }
            return UserID;
        }
        public static string? FirstName()
        {
            string? FirstName=" " ;
            if (_httpContextAccessor.HttpContext.Session.GetString("FirstName") != null)
            {
                FirstName =
               _httpContextAccessor.HttpContext.Session.GetString("FirstName").ToString();
            }

            return FirstName;
        }

    }
}
