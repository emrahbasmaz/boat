using System;

namespace Boat.Data.Dto.PaymentModule.Response
{
    [Serializable]
    public class ResponseBoatCapacity : BaseResponseMessage
    {
        public long BOAT_CAPACITY_ID { get; set; }
        public long RESERVATION_ID { get; set; }
        public long BOAT_ID { get; set; }
        public string CAPACITY { get; set; }
        public DateTime RESERVATION_DATE { get; set; }
        public DateTime RESERVATION_END_DATE { get; set; }
    }
}
