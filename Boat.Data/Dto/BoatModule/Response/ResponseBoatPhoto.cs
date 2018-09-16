using System;

namespace Boat.Data.Dto.BoatModule.Response
{
    [Serializable]
    public class ResponseBoatPhoto : BaseResponseMessage
    {
        public long PHOTO_ID { get; set; }
        public string PHOTO { get; set; }
        public long BOAT_ID { get; set; }
    }
}
