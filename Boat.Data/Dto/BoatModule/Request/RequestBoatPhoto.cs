
using System;

namespace Boat.Data.Dto.BoatModule.Request
{
    [Serializable]
    public class RequestBoatPhoto
    {
        public Header Header { get; set; }
        public string INSERT_USER { get; set; }
        public string UPDATE_USER { get; set; }
        public long PHOTO_ID { get; set; }
        public string PHOTO { get; set; }
        public long BOAT_ID { get; set; }
    }
}
