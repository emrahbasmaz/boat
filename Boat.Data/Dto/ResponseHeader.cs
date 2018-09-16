using System;
using System.Collections.Generic;
using System.Text;

namespace Boat.Data.Dto
{
    public class ResponseHeader
    {
        public string ResponseCode { get; set; }
        public bool IsSuccess { get; set; }
        public string ResponseMessage { get; set; }
        public string Token { get; set; }
    }
}
