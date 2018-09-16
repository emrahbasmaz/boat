
using Boat.Data.DataModel.CustomerModule.Entity;
using Boat.Data.DataModel.CustomerModule.Service;
using Boat.Data.DataModel.CustomerModule.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boat.Business.Common
{
    public class Tokenizer
    {
        private static TokenTransactionService tokenTransactionService;
        public static string CreateToken(long customerNumber)
        {
            tokenTransactionService = new TokenTransactionService();
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());

            TokenTransaction tt = new TokenTransaction
            {
                INSERT_USER = "SYSTEM",
                UPDATE_USER = "SYSTEM",
                CUSTOMER_NUMBER = customerNumber,
                TOKEN = token
            };
            tokenTransactionService.Insert(tt);

            return token;
        }

        public static bool checkToken(string token, long customerNumber)
        {
            tokenTransactionService = new TokenTransactionService();
            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when < DateTime.UtcNow.AddMinutes(-4))
            {
                TokenTransaction tt = new TokenTransaction();
                tt = tokenTransactionService.SelectByCustomerNumber(customerNumber);

                tokenTransactionService.Delete(tt);

                return false;
            }

            return true;
        }
    }
}
