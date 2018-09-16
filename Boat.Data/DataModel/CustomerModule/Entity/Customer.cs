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
    public class Customer
    {
        public string GUID { get; set; }
        public Int16 RECORD_STATUS { get; set; }
        public DateTime INSERT_DATE { get; set; }
        public string INSERT_USER { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        [Key]
        public long CUSTOMER_NUMBER { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_MIDDLE_NAME { get; set; }
        public string CUSTOMER_SURNAME { get; set; }
        public long IDENTIFICATION_ID { get; set; }
        public string EMAIL { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string GENDER { get; set; }
        public byte[] PASSWORD_HASH { get; set; }
        public byte[] PASSWORD_SALT { get; set; }
    }



}
