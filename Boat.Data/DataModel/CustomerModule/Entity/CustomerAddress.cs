using Boat.Backoffice.Utility;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boat.Data.DataModel.CustomerModule.Entity
{
    public class CustomerAddress
    {
        public string GUID { get; set; }
        public Int16 RECORD_STATUS { get; set; }
        public DateTime INSERT_DATE { get; set; }
        public string INSERT_USER { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        [Key]
        public long CUSTOMER_NUMBER { get; set; }
        public string CITY { get; set; }
        public string COUNTRY { get; set; }
        public string DESCRIPTION { get; set; }
        public string ZIPCODE { get; set; }
    }
}
