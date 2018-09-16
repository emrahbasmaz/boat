using Boat.Backoffice.DataModel;
using Boat.Backoffice.DataModel.CustomerModule.RequestMessages;
using Boat.Backoffice.DataModel.CustomerModule.ResponseMessages;
using Boat.Backoffice.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Boat.Backoffice.Common;
using Newtonsoft.Json;


namespace Boat.Backoffice.Controller.UserController.Login
{
    public class LoginOperation : OperationBase
    {

        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Customer customer = null;
        public RequestPersonalInformation request = new RequestPersonalInformation();

        public BaseResponseMessage baseResponseMessage = null;
        public LoginOperation(RequestPersonalInformation request) : base(OperationCounter.LoginOperation)
        {
            this.request.Header = new Header();
            this.request = request;
        }

        public override BaseResponseMessage ValidateInput()
        {
            TranSeq = Enums.TransactionSequence.VALIDATION;
            #region Validation HeaderRequest
            BaseResponseMessage resp = new BaseResponseMessage();
            resp.header = new ResponseHeader();
            if (this.request.Header.ApiKey != CommonDefinitions.APIKEY)
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.API_KEY_NOT_MATCH;
            }
            else if (this.request.Header.Device == 0)
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.DEVICE_INFORMATION_NOT_FOUND;
            }
            else if (String.IsNullOrEmpty(this.request.Header.RequestId))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.REQUEST_ID_NOT_FOUND;
            }
            //else if (!ValidateIdentificationNumber.CheckIdentificationNumber(this.request.IDENTIFICATION_ID.ToString()))
            //{
            //    resp.header.IsSuccess = false;
            //    resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
            //    resp.header.ResponseMessage = CommonDefinitions.IDENTIFICATION_ID_NOT_VALID;
            //}
            else if (String.IsNullOrEmpty(this.request.PASSWORD))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.PASSWORD_NOT_VALID;
            }
            else
            {
                resp.header.IsSuccess = true;
                resp.header.ResponseCode = CommonDefinitions.SUCCESS;
                resp.header.ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE;
            }
            #endregion
            return resp;
        }

        public override void DoOperation()
        {
            //Validate Reques Header / Constants
            this.baseResponseMessage = ValidateInput();
            if (!this.baseResponseMessage.header.IsSuccess)
                throw new Exception(this.baseResponseMessage.header.ResponseMessage);

            this.baseResponseMessage.header.Token = Tokenizer.CreateToken(this.request.CUSTOMER_NUMBER);
            this.baseResponseMessage.header.IsSuccess = true;
            this.baseResponseMessage.header.ResponseCode = CommonDefinitions.SUCCESS;
            this.baseResponseMessage.header.ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE;


            this.customer = Customer.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);

            if (!VerifyPasswordHash(this.request.PASSWORD, this.customer.PASSWORD_HASH, this.customer.PASSWORD_SALT))
            {
                this.baseResponseMessage.header.IsSuccess = false;
                this.baseResponseMessage.header.ResponseCode = CommonDefinitions.INTERNAL_PASSWORD_ERROR;
                this.baseResponseMessage.header.ResponseMessage = CommonDefinitions.PASSWORD_NOT_VALID;
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public override void RollbackOperation()
        {
            switch (TranSeq)
            {
                case Enums.TransactionSequence.DEFAULT:
                    break;
                case Enums.TransactionSequence.VALIDATION:
                    break;
                case Enums.TransactionSequence.DO_OPERATION:
                    break;
                case Enums.TransactionSequence.COMPLETED:
                    break;
                case Enums.TransactionSequence.ROLLBACK:
                    break;
                default:
                    break;
            }
            TranSeq = Enums.TransactionSequence.ROLLBACK;
        }


    }
}
