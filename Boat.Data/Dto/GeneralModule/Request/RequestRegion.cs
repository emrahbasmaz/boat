using System;

namespace Boat.Data.Dto.GeneralModule.Request
{
    [Serializable]
    public class RequestRegion
    {
        public Header Header { get; set; }
        public string INSERT_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public long REGION_ID { get; set; }
        public string REGION_NAME { get; set; }
    }
}
