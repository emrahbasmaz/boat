using System;

namespace Boat.Data.Dto.CustomerModule.Response
{
    [Serializable]
    public class ResponseCustomerAddress : BaseResponseMessage
    {
        public long CUSTOMER_NUMBER { get; set; }
        public string CITY { get; set; }
        public string COUNTRY { get; set; }
        public string DESCRIPTION { get; set; }
        public string ZIPCODE { get; set; }
    }
}
