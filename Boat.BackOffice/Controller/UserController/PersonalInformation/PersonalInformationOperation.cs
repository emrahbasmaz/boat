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

namespace Boat.Backoffice.Controller.UserController.PersonalInformation
{
    public class PersonalInformationOperation : OperationBase
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Customer customer = null;
        public RequestPersonalInformation request = new RequestPersonalInformation();
        public ResponsePersonalInformation response = null;
        public BaseResponseMessage baseResponseMessage = null;
        public PersonalInformationOperation(RequestPersonalInformation request)
        {
            this.request.Header = new Header();
            this.request = request;
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
            else if (String.IsNullOrEmpty(this.request.PASSWORD))
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
                CreatePasswordHash(this.request.PASSWORD, out passwordHash, out passwordSalt);
                
                //Operation
                switch (this.request.Header.OperationTypes)
                {
                    case (int)OperationType.OperationTypes.ADD:
                        long checkGuid = 0;
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
                        checkGuid = Customer.Insert(this.customer);
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
                        this.customer = Customer.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);
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
                            PASSWORD = this.customer.PASSWORD_HASH.ToString(),
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
                        this.customer = Customer.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);
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
                            //PASSWORD_HASH = Convert.ToByte(this.request.PASSWORD),
                            GENDER = this.request.GENDER,
                        };
                        //Update Customer Information
                        Customer.Update(this.customer);

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
                        this.customer = Customer.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);
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
                        Customer.Delete(this.customer);

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



            }
            catch (Exception ex)
            {
                string operationError = "HATA:[" + "CustomerNumber:" + this.request.CUSTOMER_NUMBER + ",ResponseCode:" + this.baseResponseMessage.header.ResponseCode + ", ResponseMessage:" + ex.Message + "]";
                log.InfoFormat(operationError, ex);
                throw new Exception(ex.Message, ex.InnerException);
            }


        }

        public override void RollbackOperation()
        {
            throw new NotImplementedException();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


    }
}
