using Boat.Business.Common;
using Boat.Business.Framework;
using Boat.Data;
using Boat.Data.DataModel.CustomerModule.Entity;
using Boat.Data.DataModel.CustomerModule.Service;
using Boat.Data.DataModel.CustomerModule.Service.Interface;
using Boat.Data.Dto;
using Boat.Data.Dto.CustomerModule.Request;
using Boat.Data.Dto.CustomerModule.Response;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Boat.Business.Operation.UserOperation.PersonalInformation
{
    public class PersonalInformationOperation : OperationBase
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ICustomerService customerService;
        public Customer customer = null;
        public RequestPersonalInformation request = new RequestPersonalInformation();
        public ResponsePersonalInformation response = null;
        public BaseResponseMessage baseResponseMessage = null;

        public PersonalInformationOperation(RequestPersonalInformation request, CustomerService service)
        {
            this.request.Header = new Header();
            this.request = request;

            this.customerService = service;
        }

        public override BaseResponseMessage ValidateInput()
        {
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
            else if (!ValidateIdentificationNumber.CheckIdentificationNumber(this.request.IDENTIFICATION_ID.ToString()))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.IDENTIFICATION_ID_NOT_VALID;
            }
            else if (this.request.PASSWORD_HASH.Length < 1)
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.PASSWORD_NOT_VALID;
            }
            else if (this.request.Header.OperationTypes != (int)OperationType.OperationTypes.ADD)
            {
                if (!Tokenizer.checkToken(this.request.Header.TokenId, this.request.CUSTOMER_NUMBER))
                {
                    resp.header.IsSuccess = false;
                    resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                    resp.header.ResponseMessage = CommonDefinitions.TOKEN_NOT_VALID;
                }
                else
                {
                    resp.header.IsSuccess = true;
                    resp.header.ResponseCode = CommonDefinitions.SUCCESS;
                    resp.header.ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE;
                }
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
            try
            {
                //Validate Reques Header / Constants
                this.baseResponseMessage = ValidateInput();
                if (!this.baseResponseMessage.header.IsSuccess)
                    throw new Exception(this.baseResponseMessage.header.ResponseMessage);
                byte[] passwordHash, passwordSalt;

                //Operation
                switch (this.request.Header.OperationTypes)
                {
                    case (int)OperationType.OperationTypes.ADD:
                        long checkGuid = 0;
                        //hash algorithm
                        CreatePasswordHash(this.request.PASSWORD_HASH.ToString(), out passwordHash, out passwordSalt);

                        this.customer = new Customer
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            CUSTOMER_NAME = this.request.CUSTOMER_NAME,
                            CUSTOMER_SURNAME = this.request.CUSTOMER_SURNAME,
                            CUSTOMER_MIDDLE_NAME = this.request.CUSTOMER_MIDDLE_NAME,
                            CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                            IDENTIFICATION_ID = this.request.IDENTIFICATION_ID,
                            EMAIL = this.request.EMAIL,
                            PHONE_NUMBER = this.request.PHONE_NUMBER,
                            PASSWORD_HASH = passwordHash,
                            PASSWORD_SALT = passwordSalt,
                            GENDER = this.request.GENDER,
                        };
                        //Add Data to Database
                        checkGuid = this.customerService.Insert(this.customer);
                        string token = Tokenizer.CreateToken(checkGuid);
                        this.response = new ResponsePersonalInformation
                        {
                            INSERT_USER = this.customer.INSERT_USER,
                            UPDATE_USER = this.customer.UPDATE_USER,
                            CUSTOMER_NAME = this.customer.CUSTOMER_NAME,
                            CUSTOMER_SURNAME = this.customer.CUSTOMER_SURNAME,
                            CUSTOMER_MIDDLE_NAME = this.customer.CUSTOMER_MIDDLE_NAME,
                            CUSTOMER_NUMBER = this.customer.CUSTOMER_NUMBER,
                            IDENTIFICATION_ID = this.customer.IDENTIFICATION_ID,
                            EMAIL = this.customer.EMAIL,
                            PHONE_NUMBER = this.customer.PHONE_NUMBER,
                            GENDER = this.request.GENDER,
                            header = new ResponseHeader
                            {
                                IsSuccess = checkGuid == 0 ? false : true,
                                ResponseCode = checkGuid == 0 ? CommonDefinitions.INTERNAL_SYSTEM_UNKNOWN_ERROR : CommonDefinitions.SUCCESS,
                                ResponseMessage = checkGuid == 0 ? CommonDefinitions.ERROR_MESSAGE : CommonDefinitions.SUCCESS_MESSAGE,
                                Token = token
                            }
                        };

                        break;
                    case (int)OperationType.OperationTypes.GET:
                        //Get Data
                        this.customer = customerService.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);
                        this.response = new ResponsePersonalInformation
                        {
                            INSERT_USER = this.customer.INSERT_USER,
                            UPDATE_USER = this.customer.UPDATE_USER,
                            CUSTOMER_NAME = this.customer.CUSTOMER_NAME,
                            CUSTOMER_SURNAME = this.customer.CUSTOMER_SURNAME,
                            CUSTOMER_MIDDLE_NAME = this.customer.CUSTOMER_MIDDLE_NAME,
                            CUSTOMER_NUMBER = this.customer.CUSTOMER_NUMBER,
                            IDENTIFICATION_ID = this.customer.IDENTIFICATION_ID,
                            EMAIL = this.customer.EMAIL,
                            PHONE_NUMBER = this.customer.PHONE_NUMBER,
                            PASSWORD_HASH = this.customer.PASSWORD_HASH,
                            PASSWORD_SALT = this.customer.PASSWORD_SALT,
                            GENDER = this.request.GENDER,
                            header = new ResponseHeader
                            {
                                IsSuccess = true,
                                ResponseCode = CommonDefinitions.SUCCESS,
                                ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                            }
                        };

                        break;
                    case (int)OperationType.OperationTypes.UPDATE:

                        CreatePasswordHash(this.request.PASSWORD_HASH.ToString(), out passwordHash, out passwordSalt);

                        this.customer = customerService.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);
                        this.customer = new Customer
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            CUSTOMER_NAME = this.request.CUSTOMER_NAME,
                            CUSTOMER_SURNAME = this.request.CUSTOMER_SURNAME,
                            CUSTOMER_MIDDLE_NAME = this.request.CUSTOMER_MIDDLE_NAME,
                            CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                            IDENTIFICATION_ID = this.request.IDENTIFICATION_ID,
                            EMAIL = this.request.EMAIL,
                            PHONE_NUMBER = this.request.PHONE_NUMBER,
                            PASSWORD_HASH = this.customer.PASSWORD_HASH,
                            PASSWORD_SALT = this.customer.PASSWORD_SALT,
                            GENDER = this.request.GENDER,
                        };
                        //Update Customer Information
                        customerService.Update(this.customer);

                        this.response = new ResponsePersonalInformation
                        {
                            INSERT_USER = this.customer.INSERT_USER,
                            UPDATE_USER = this.customer.UPDATE_USER,
                            CUSTOMER_NAME = this.customer.CUSTOMER_NAME,
                            CUSTOMER_SURNAME = this.customer.CUSTOMER_SURNAME,
                            CUSTOMER_MIDDLE_NAME = this.customer.CUSTOMER_MIDDLE_NAME,
                            CUSTOMER_NUMBER = this.customer.CUSTOMER_NUMBER,
                            IDENTIFICATION_ID = this.customer.IDENTIFICATION_ID,
                            EMAIL = this.customer.EMAIL,
                            PHONE_NUMBER = this.customer.PHONE_NUMBER,
                            GENDER = this.request.GENDER,
                            header = new ResponseHeader
                            {
                                IsSuccess = true,
                                ResponseCode = CommonDefinitions.SUCCESS,
                                ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                            }
                        };

                        break;
                    case (int)OperationType.OperationTypes.DELETE:
                        this.customer = customerService.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);
                        this.customer = new Customer
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            RECORD_STATUS = 0,
                            UPDATE_USER = this.request.UPDATE_USER,
                            CUSTOMER_NAME = this.request.CUSTOMER_NAME,
                            CUSTOMER_SURNAME = this.request.CUSTOMER_SURNAME,
                            CUSTOMER_MIDDLE_NAME = this.request.CUSTOMER_MIDDLE_NAME,
                            CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                            IDENTIFICATION_ID = this.request.IDENTIFICATION_ID,
                            EMAIL = this.request.EMAIL,
                            PHONE_NUMBER = this.request.PHONE_NUMBER,
                            GENDER = this.request.GENDER,
                        };
                        //Update Customer to Passive
                        customerService.Delete(this.customer);

                        this.response = new ResponsePersonalInformation
                        {
                            INSERT_USER = this.customer.INSERT_USER,
                            UPDATE_USER = this.customer.UPDATE_USER,
                            CUSTOMER_NAME = this.customer.CUSTOMER_NAME,
                            CUSTOMER_SURNAME = this.customer.CUSTOMER_SURNAME,
                            CUSTOMER_MIDDLE_NAME = this.customer.CUSTOMER_MIDDLE_NAME,
                            CUSTOMER_NUMBER = this.customer.CUSTOMER_NUMBER,
                            IDENTIFICATION_ID = this.customer.IDENTIFICATION_ID,
                            EMAIL = this.customer.EMAIL,
                            PHONE_NUMBER = this.customer.PHONE_NUMBER,
                            GENDER = this.request.GENDER,
                            header = new ResponseHeader
                            {
                                IsSuccess = true,
                                ResponseCode = CommonDefinitions.SUCCESS,
                                ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                            }
                        };

                        break;

                    default:
                        break;

                }
                passwordHash = null;
                passwordSalt = null;
            }
            catch (Exception ex)
            {
                string operationError = "HATA:[" + "CustomerNumber:" + this.request.CUSTOMER_NUMBER + ",ResponseCode:" + this.baseResponseMessage.header.ResponseCode + ", ResponseMessage:" + ex.Message + "]";
                log.InfoFormat(operationError, ex);
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        private void CreatePasswordHash(string v, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(v));
            }
        }

        public override void RollbackOperation()
        {
            throw new NotImplementedException();
        }


    }
}
