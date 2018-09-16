using System;

namespace Boat.Data.Dto.PaymentModule.Response
{
    public class ResponseConfirmReservation : BaseResponseMessage
    {
        public long RESERVATION_ID { get; set; }
        public Int16 CONFIRM { get; set; }
    }
}
