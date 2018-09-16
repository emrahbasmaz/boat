using System;

namespace Boat.Data.Dto.PaymentModule.Request
{
    [Serializable]
    public class RequestPayment 
    {
        public Header Header { get; set; }
        public long BOAT_ID { get; set; }
        public string CONVERSATION_ID { get; set; }
        public string PAYMENT_ID { get; set; }
        public long CUSTOMER_NUMBER { get; set; }
        public string CARD_HOLDER_NAME { get; set; }
        public string CARD_REF_NUMBER { get; set; }
        public string PRICE { get; set; }
        public string PAID_PRICE { get; set; }
        public string PAYMENT_CHANNEL { get; set; }
        public string CALLBACK_URL { get; set; }
        public string IP { get; set; }
        public string CURRENCY { get; set; }
        public string TOUR_TYPE { get; set; }
        public string PAYMENT_TYPE { get; set; }

        public DateTime RESERVATION_DATE { get; set; }
        public DateTime RESERVATION_END_DATE { get; set; }
        public long RESERVATION_ID { get; set; }

        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string BaseUrl { get; set; }

        public RequestCard PaymentCard { get; set; }
    }
}
