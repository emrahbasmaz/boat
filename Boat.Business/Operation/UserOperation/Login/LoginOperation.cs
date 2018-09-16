using Boat.Business.Common;
using Boat.Business.Framework;
using Boat.Data;
using Boat.Data.DataModel.CustomerModule.Entity;
using Boat.Data.DataModel.CustomerModule.Service;
using Boat.Data.DataModel.CustomerModule.Service.Interface;
using Boat.Data.Dto;
using Boat.Data.Dto.CustomerModule.Request;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Enums = Boat.Business.Framework.Enums;

namespace Boat.Business.Operation.UserOperation.Login
{
    public class LoginOperation : OperationBase
    {
        private ICustomerService customerService;
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Customer customer = null;
        public RequestPersonalInformation request = new RequestPersonalInformation();

        public BaseResponseMessage baseResponseMessage = null;
        public LoginOperation(RequestPersonalInformation request, CustomerService customerService) : base(OperationCounter.LoginOperation)
        {
            this.request.Header = new Header();
            this.request = request;
            this.customerService = customerService;
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
            else if (this.request.PASSWORD_HASH.Length < 1)
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


            this.customer = customerService.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);

            if (!VerifyPasswordHash(this.request.PASSWORD_HASH.ToString(), this.customer.PASSWORD_HASH, this.customer.PASSWORD_SALT))
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
