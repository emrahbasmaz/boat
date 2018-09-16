using System;

namespace Boat.Data.Dto.PaymentModule.Response
{
    [Serializable]
    public class ResponsePayment : BaseResponseMessage
    {
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
    }
}
