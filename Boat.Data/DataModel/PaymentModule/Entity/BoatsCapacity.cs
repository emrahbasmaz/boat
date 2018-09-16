using Boat.Backoffice.Utility;
using Boat.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Boat.Backoffice.DataModel.PaymentModule.Entity
{
    public class BoatsCapacity
    {
        public string GUID { get; set; }
        public Int16 RECORD_STATUS { get; set; }
        public DateTime INSERT_DATE { get; set; }
        public string INSERT_USER { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        [Key]
        public long BOAT_CAPACITY_ID { get; set; }
        public long RESERVATION_ID { get; set; }
        public long BOAT_ID { get; set; }
        public string CAPACITY { get; set; }
        public DateTime RESERVATION_DATE { get; set; }
        public DateTime RESERVATION_END_DATE { get; set; }
    }
}
