using System;
using System.Collections.Generic;
using System.Text;

namespace Boat.Data.Dto
{
    [Serializable]
    public class Header
    {
        public string RequestId { get; set; }
        public string ApiKey { get; set; }
        public int Device { get; set; }
        public int OperationTypes { get; set; }
        public string TokenId { get; set; }
    }
}
