using Boat.Backoffice.Utility;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Boat.Data.DataModel.GeneralModule.Entity
{
    public class Complaints
    {
        public string GUID { get; set; }
        public Int16 RECORD_STATUS { get; set; }
        public DateTime INSERT_DATE { get; set; }
        public string INSERT_USER { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        [Key]
        public long COMPLAINT_ID { get; set; }
        public string CONTENT_HEADER { get; set; }
        public string CONTENT_TEXT { get; set; }
        public long RESERVATION_ID { get; set; }
        public long CUSTOMER_NUMBER { get; set; }
        public string EMAIL { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string PHOTO { get; set; }
        public Int16 CONFIRM { get; set; }
    }
}
