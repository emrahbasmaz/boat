using System;
using System.Collections.Generic;
using System.Text;

namespace Boat.Data.Dto.GeneralModule.Response
{

    public class ResponseFavorites : BaseResponseMessage
    {
        public long CUSTOMER_NUMBER { get; set; }
        public long BOAT_ID { get; set; }
    }
}
