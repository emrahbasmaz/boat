using System;

namespace Boat.Data.Dto.BoatModule.Response
{
    [Serializable]
    public class ResponseCaptain : BaseResponseMessage
    {
        public long BOAT_ID { get; set; }
        public long CAPTAIN_ID { get; set; }
        public string CAPTAIN_NAME { get; set; }
        public string CAPTAIN_MIDDLE_NAME { get; set; }
        public string CAPTAIN_SURNAME { get; set; }
        public string CAPTAIN_INFO { get; set; }
        public string PHONE_NUMBER { get; set; }
        public long IDENTIFICATION_ID { get; set; }
        public string EMAIL { get; set; }
    }
}
