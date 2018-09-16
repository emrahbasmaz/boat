using System;

namespace Boat.Data.Dto.GeneralModule.Request
{
    [Serializable]
    public class RequestFavorites
    {
        public Header Header { get; set; }
        public string INSERT_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public long CUSTOMER_NUMBER { get; set; }
        public long BOAT_ID { get; set; }

        public bool IsDeleteByCustomerNumber { get; set; }
    }
}
