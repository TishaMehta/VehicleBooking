namespace VehicleBooking.DAL
{
    public class Dal_Helper
    {
        public static string ConnStr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("myConnectionString");

    }
}
