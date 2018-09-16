using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boat.Backoffice.Utility
{
    public class Constant
    {
        static string conString = "Data Source=94.73.170.20;database=boatDB;User ID=deem;Password=CRwn61Z9;";
        public static string DatabaseConnection = conString;
        //ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;


    }
}
