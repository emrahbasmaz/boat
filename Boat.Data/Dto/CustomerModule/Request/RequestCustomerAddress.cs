using System;

namespace Boat.Data.Dto.CustomerModule.Request
{
    [Serializable]
    public class RequestCustomerAddress
    {
        public Header Header { get; set; }
        public string INSERT_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public long CUSTOMER_NUMBER { get; set; }
        public string CITY { get; set; }
        public string COUNTRY { get; set; }
        public string DESCRIPTION { get; set; }
        public string ZIPCODE { get; set; }
    }
}
