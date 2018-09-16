using Boat.Data.DataModel.GeneralModule.Service.Interface;
using System;

namespace Boat.Business.Common
{
    public class SequenceManager
    {
        private const string NextSequenceSql = "DECLARE @value BIGINT;EXECUTE dbo.GetSEQUENCE @value OUTPUT;SELECT @value AS ID ";
        private const string RestartSequenceSql = "ALTER SEQUENCE {0} RESTART";

        private static ISequenceNumberService sequenceNumberService;
        public static long GetNextPaymentValue()
        {
            try
            {
                long nextValue = sequenceNumberService.SelectByNewId();

                return nextValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static long GetNextCustomerValue()
        {
            try
            {
                long nextValue = sequenceNumberService.SelectByNewCustomerId();

                return nextValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public static void Restart(string sequenceName)
        //{
        //    try
        //    {
        //        var parameters = new List<DynamicParameters>();
        //        var p = new DynamicParameters();
        //        // p.Add("@Kind", InvoiceKind.WebInvoice, DbType.Int32, ParameterDirection.Input);
        //        p.Add("@ResultNumber", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
        //        parameters.Add(p);

        //        using (var sqlConnection = new SqlConnection(Constant.DatabaseConnection))
        //        {
        //            sqlConnection.Execute(string.Format(RestartSequenceSql, sequenceName), parameters, commandType: CommandType.StoredProcedure);
        //            var rowNumber = parameters.Sum(x => x.Get<int>("@ResultNumber"));

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("UnableToRestartSequence");
        //    }
        //}

        public enum InvoiceKind
        {
            StoreInvoice = 1,
            WebInvoice = 2
        }
    }
}
