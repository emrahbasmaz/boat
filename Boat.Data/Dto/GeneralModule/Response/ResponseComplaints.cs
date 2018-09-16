using System;

namespace Boat.Data.Dto.GeneralModule.Response
{
    public class ResponseComplaints : BaseResponseMessage
    {
        public string INSERT_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public long COMPLAINT_ID { get; set; }
        public string CONTENT_HEADER { get; set; }
        public string CONTENT_TEXT { get; set; }
        public long RESERVATION_ID { get; set; }
        public long CUSTOMER_NUMBER { get; set; }
        public string EMAIL { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string PHOTO { get; set; }
        public Int16 CONFIRM { get; set; }
    }
}
