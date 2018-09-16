using System;

namespace Boat.Data.Dto.PaymentModule.Request
{
    [Serializable]
    public class RequestConfirmReservation
    {
        public Header Header { get; set; }
        public long RESERVATION_ID { get; set; }
        public Int16 CONFIRM { get; set; }
    }
}
