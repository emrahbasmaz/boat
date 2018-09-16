using System;

namespace Boat.Data.Dto.PaymentModule.Request
{
    [Serializable]
    public class RequestCard
    {
        public Header Header { get; set; }
        public string CONVERSATION_ID { get; set; }
        public long CUSTOMER_NUMBER { get; set; }
        public string CARD_ALIAS { get; set; }
        public string CARD_HOLDER_NAME { get; set; }
        public string CARD_REF_NUMBER { get; set; }
        public string CARD_MASKED_NUMBER { get; set; }
        public string CARD_EXPIRE_YEAR { get; set; }
        public string CARD_EXPIRE_MONTH { get; set; }
        public string CARD_EXPIRE_DATE { get; set; }
        public string CARD_CVV { get; set; }
    }
}
