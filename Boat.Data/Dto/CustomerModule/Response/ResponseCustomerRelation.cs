using System;

namespace Boat.Data.Dto.CustomerModule.Response
{
    [Serializable]
    public class ResponseCustomerRelation : BaseResponseMessage
    {
        public string INSERT_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public long RELATION_NUMBER { get; set; }
        public long CUSTOMER_NUMBER { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_MIDDLE_NAME { get; set; }
        public string CUSTOMER_SURNAME { get; set; }
        public long IDENTIFICATION_ID { get; set; }
        public string EMAIL { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string GENDER { get; set; }
    }
}
