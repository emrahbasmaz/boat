using System;

namespace Boat.Data.Dto.PaymentModule.Request
{
    [Serializable]
    public class RequestReservation
    {
        public Header Header { get; set; }
        public string INSERT_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public long RESERVATION_ID { get; set; }
        public string PAYMENT_ID { get; set; }//BASKET_ID
        public long CUSTOMER_NUMBER { get; set; }
        public long BOAT_ID { get; set; }
        public string PRICE { get; set; }
        public string TOUR_TYPE { get; set; }
        public string PAYMENT_TYPE { get; set; }

        public string CAPACITY { get; set; }
        public DateTime RESERVATION_DATE { get; set; }
        public DateTime RESERVATION_END_DATE { get; set; }

        public Int16 CONFIRM { get; set; }
    }
}
