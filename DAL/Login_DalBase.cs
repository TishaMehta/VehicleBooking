using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;
using VehicleBooking.Areas.LOC_UserAdmin.Models;

namespace VehicleBooking.DAL
{
    public class Login_DalBase : Dal_Helper
    {
        public UserAdminModel SignIn(string FirstName, string password)
        {
            SqlDatabase db = new SqlDatabase(ConnStr);
            DbCommand cmd = db.GetStoredProcCommand("PR_SEC_User_Login");
            db.AddInParameter(cmd, "@EmailOrUserName", SqlDbType.VarChar, FirstName);
            db.AddInParameter(cmd, "@Password", SqlDbType.VarChar, password);
            UserAdminModel userModel = new UserAdminModel();
            using (IDataReader reader = db.ExecuteReader(cmd))
            {

                if (reader.Read())
                {
                    userModel.UserID = Convert.ToInt32(reader["UserID"]);
                    userModel.FirstName = reader["FirstName"].ToString();
                    userModel.LastName = reader["LastName"].ToString();
                    userModel.Email = reader["Email"].ToString();
                    userModel.PhoneNumber = reader["PhoneNumber"].ToString();
                    userModel.Address = reader["Address"].ToString();
                    userModel.Password = reader["Password"].ToString();
                    userModel.IsAdmin = Convert.ToBoolean(reader["IsAdmin"]);

                }
                else
                {
                    return null;
                }
            }

            return userModel;
        }

    }
}
