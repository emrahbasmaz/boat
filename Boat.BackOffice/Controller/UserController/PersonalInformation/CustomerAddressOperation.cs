using Boat.Backoffice.Common;
using Boat.Backoffice.DataModel.CustomerModule.Entity;
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

namespace Boat.Backoffice.Controller.UserController.PersonalInformation
{
    public class CustomerAddressOperation : OperationBase
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public CustomerAddress customerAddress = null;
        public RequestCustomerAddress request = new RequestCustomerAddress();
        public ResponseCustomerAddress response = null;

        public BaseResponseMessage baseResponseMessage = null;

        public CustomerAddressOperation(RequestCustomerAddress request)
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
            else if (String.IsNullOrEmpty(this.request.CITY))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.CITY_NOT_FOUND;
            }
            else if (String.IsNullOrEmpty(this.request.COUNTRY))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.COUNTRY_NOT_FOUND;
            }
            else if (String.IsNullOrEmpty(this.request.DESCRIPTION))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.DESCRIPTON_NOT_FOUND;
            }
            else if (String.IsNullOrEmpty(this.request.ZIPCODE))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.ZIPCODE_NOT_FOUND;
            }
            else if (String.IsNullOrEmpty(this.request.Header.TokenId))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.TOKEN_NOT_VALID;
            }
            else if (!Tokenizer.checkToken(this.request.Header.TokenId, this.request.CUSTOMER_NUMBER))
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
            #endregion
            return resp;
        }

        public override void DoOperation()
        {
            try
            { //Validate Reques Header / Constants
                this.baseResponseMessage = ValidateInput();
                if (!this.baseResponseMessage.header.IsSuccess)
                    throw new Exception(this.baseResponseMessage.header.ResponseMessage);
                //Operation
                switch (this.request.Header.OperationTypes)
                {
                    case (int)OperationType.OperationTypes.ADD:
                        #region ADD
                        long checkGuid = 0;
                        this.customerAddress = new CustomerAddress
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                            CITY = this.request.CITY,
                            COUNTRY = this.request.COUNTRY,
                            DESCRIPTION = this.request.DESCRIPTION,
                            ZIPCODE = this.request.ZIPCODE
                        };
                        //Add Data to Database
                        checkGuid = CustomerAddress.Insert(this.customerAddress);

                        this.response = new ResponseCustomerAddress
                        {
                            CUSTOMER_NUMBER = checkGuid,
                            ZIPCODE = this.request.ZIPCODE,
                            DESCRIPTION = this.request.DESCRIPTION,
                            COUNTRY = this.request.COUNTRY,
                            CITY = this.request.CITY,
                            header = new ResponseHeader
                            {
                                IsSuccess = checkGuid == 0 ? false : true,
                                ResponseCode = checkGuid == 0 ? CommonDefinitions.INTERNAL_SYSTEM_UNKNOWN_ERROR : CommonDefinitions.SUCCESS,
                                ResponseMessage = checkGuid == 0 ? CommonDefinitions.ERROR_MESSAGE : CommonDefinitions.SUCCESS_MESSAGE
                            }
                        };
                        #endregion
                        break;
                    case (int)OperationType.OperationTypes.GET:
                        #region GET
                        customerAddress = CustomerAddress.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);
                        this.response = new ResponseCustomerAddress
                        {
                            CUSTOMER_NUMBER = customerAddress.CUSTOMER_NUMBER,
                            CITY = customerAddress.CITY,
                            COUNTRY = customerAddress.COUNTRY,
                            DESCRIPTION = customerAddress.DESCRIPTION,
                            ZIPCODE = customerAddress.ZIPCODE,
                            header = new ResponseHeader
                            {
                                IsSuccess = true,
                                ResponseCode = CommonDefinitions.SUCCESS,
                                ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                            }
                        };
                        #endregion  
                        break;
                    case (int)OperationType.OperationTypes.DELETE:
                        #region DELETE
                        this.customerAddress = new CustomerAddress
                        {
                            CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                            ZIPCODE = this.request.ZIPCODE,
                            DESCRIPTION = this.request.DESCRIPTION,
                            CITY = this.request.CITY,
                            COUNTRY = this.request.COUNTRY,
                            UPDATE_USER = this.request.UPDATE_USER

                        };
                        if (CustomerAddress.Delete(this.customerAddress))
                        {
                            this.response = new ResponseCustomerAddress
                            {
                                CUSTOMER_NUMBER = customerAddress.CUSTOMER_NUMBER,
                                CITY = "",
                                COUNTRY = "",
                                DESCRIPTION = "",
                                ZIPCODE = "",
                                header = new ResponseHeader
                                {
                                    IsSuccess = true,
                                    ResponseCode = CommonDefinitions.SUCCESS,
                                    ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                                }
                            };
                        }
                        else
                        {
                            this.response = new ResponseCustomerAddress
                            {
                                CUSTOMER_NUMBER = customerAddress.CUSTOMER_NUMBER,
                                CITY = customerAddress.CITY,
                                COUNTRY = customerAddress.COUNTRY,
                                DESCRIPTION = customerAddress.DESCRIPTION,
                                ZIPCODE = customerAddress.ZIPCODE,
                                header = new ResponseHeader
                                {
                                    IsSuccess = false,
                                    ResponseCode = CommonDefinitions.ERROR_MESSAGE,
                                    ResponseMessage = CommonDefinitions.INTERNAL_SYSTEM_UNKNOWN_ERROR
                                }
                            };
                        }
                        #endregion
                        break;
                    case (int)OperationType.OperationTypes.UPDATE:
                        #region UPDATE
                        this.customerAddress = new CustomerAddress
                        {
                            CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                            ZIPCODE = this.request.ZIPCODE,
                            DESCRIPTION = this.request.DESCRIPTION,
                            CITY = this.request.CITY,
                            COUNTRY = this.request.COUNTRY,
                            UPDATE_USER = this.request.UPDATE_USER

                        };
                        if (CustomerAddress.Update(this.customerAddress))
                        {
                            this.response = new ResponseCustomerAddress
                            {
                                CUSTOMER_NUMBER = customerAddress.CUSTOMER_NUMBER,
                                CITY = customerAddress.CITY,
                                COUNTRY = customerAddress.COUNTRY,
                                DESCRIPTION = customerAddress.DESCRIPTION,
                                ZIPCODE = customerAddress.ZIPCODE,
                                header = new ResponseHeader
                                {
                                    IsSuccess = true,
                                    ResponseCode = CommonDefinitions.SUCCESS,
                                    ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                                }
                            };
                        }
                        else
                        {
                            this.response = new ResponseCustomerAddress
                            {
                                CUSTOMER_NUMBER = customerAddress.CUSTOMER_NUMBER,
                                CITY = customerAddress.CITY,
                                COUNTRY = customerAddress.COUNTRY,
                                DESCRIPTION = customerAddress.DESCRIPTION,
                                ZIPCODE = customerAddress.ZIPCODE,
                                header = new ResponseHeader
                                {
                                    IsSuccess = false,
                                    ResponseCode = CommonDefinitions.ERROR_MESSAGE,
                                    ResponseMessage = CommonDefinitions.INTERNAL_SYSTEM_UNKNOWN_ERROR
                                }
                            };
                        }
                        #endregion
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }

        public override void RollbackOperation()
        {
            throw new NotImplementedException();
        }


    }
}
