

namespace Boat.Business.Common
{
    public class Attribute
    {
        internal class OperationCode : Attribute
        {
            public OperationCode(string code)
            {
                this.Code = code;
            }
            public string Code { get; set; }

        }
    }
}
