using System;
using System.Collections.Generic;

namespace Boat.Data.Dto.BoatModule.Response
{
    [Serializable]
    public class ResponseBoats : BaseResponseMessage
    {
        public string INSERT_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public long BOAT_ID { get; set; }
        public string BOAT_NAME { get; set; }
        public string FLAG { get; set; }
        public Int32 QUANTITY { get; set; }
        public string ROTA_INFO { get; set; }
        public string BOAT_INFO { get; set; }
        public long CAPTAIN_ID { get; set; }
        public long REGION_ID { get; set; }
        public string REGION_NAME { get; set; }
        public string PRICE { get; set; }
        public string TOUR_TYPE { get; set; }
        public string PRIVATE_PRICE { get; set; }
        public List<ResponseBoats> AllBoats { get; set; }

    }
}
