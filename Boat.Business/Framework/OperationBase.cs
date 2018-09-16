using Boat.Data.Dto;
using log4net;
using System;
using System.Reflection;
using static Boat.Business.Framework.Enums;

namespace Boat.Business.Framework
{
    public abstract class OperationBase
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected TransactionSequence TranSeq { get; set; }
        public string OperationCode { get; set; }

        //Constructor
        public OperationBase()
        {

        }

        //Validation
        public abstract BaseResponseMessage ValidateInput();

        //Operation
        public abstract void DoOperation();

        //Rollback
        public abstract void RollbackOperation();

        //Call or Execute Method
        public void Execute()
        {
            try
            {
                TranSeq = TransactionSequence.DO_OPERATION;
                DoOperation();

            }
            catch (Exception ex)
            {
                string systemError = "HATA:[" + ex.Message + "]";
                RollbackOperation();
                try { log.Error(systemError); }
                catch { }
                throw new Exception(systemError);
            }
        }

        public OperationBase(string operationCode)
        {
            OperationCode = operationCode;

            TranSeq = TransactionSequence.DEFAULT;
        }

    }
}
