using System;

namespace Boat.Data.Dto.PaymentModule.Response
{
    [Serializable]
    public class ResponseCard : BaseResponseMessage
    {
        public string CARD_ALIAS { get; set; }
        public string CARD_HOLDER_NAME { get; set; }
        public string CARD_REF_NUMBER { get; set; }
    }
}
