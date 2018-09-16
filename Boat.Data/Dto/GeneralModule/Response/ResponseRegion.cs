using System;

namespace Boat.Data.Dto.GeneralModule.Response
{
    [Serializable]
    public class ResponseRegion : BaseResponseMessage
    {
        public string INSERT_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public long REGION_ID { get; set; }
        public string REGION_NAME { get; set; }
    }
}
