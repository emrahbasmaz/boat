using Dapper.Contrib.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boat.Backoffice.Utility;

namespace Boat.Data.DataModel.GeneralModule.Entity
{
    public class SequenceCustomerNumber
    {
        [Key]
        public long ID { get; set; }
       
    }
}
