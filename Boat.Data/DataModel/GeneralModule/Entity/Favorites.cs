using Boat.Backoffice.Utility;
using Boat.Data.DataModel.BoatModule.Entity;
using Boat.Data.DataModel.CustomerModule.Entity;
using Boat.Data.Dto.BoatModule.Response;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace Boat.Data.DataModel.GeneralModule.Entity
{
    public class Favorites
    {
        public string GUID { get; set; }
        public Int16 RECORD_STATUS { get; set; }
        public DateTime INSERT_DATE { get; set; }
        public string INSERT_USER { get; set; }
        public DateTime UPDATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        [Key]
        public long FAVORITE_ID { get; set; }
        public long CUSTOMER_NUMBER { get; set; }
        public long BOAT_ID { get; set; }
    }
}
