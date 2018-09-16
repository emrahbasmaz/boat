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
    public class CustomerRelationOperation : OperationBase
    {
        private CustomerRelationService customerRelationService;
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CustomerRelation customer = null;
        public List<CustomerRelation> listcustomerRelation = null;
        public RequestCustomerRelation request = new RequestCustomerRelation();
        public List<ResponseCustomerRelation> listcustomerRelationResponse = null;
        public ResponseCustomerRelation responsedata = null;
        public List<ResponseCustomerRelation> response = null;
        public BaseResponseMessage baseResponseMessage = null;

        public CustomerRelationOperation(RequestCustomerRelation request)
        {
            this.request.Header = new Header();
            this.request = request;
            this.customerRelationService = new CustomerRelationService();
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
            {
                //Validate Reques Header / Constants
                this.baseResponseMessage = ValidateInput();
                if (!this.baseResponseMessage.header.IsSuccess)
                    throw new Exception(this.baseResponseMessage.header.ResponseMessage);
                //Operation
                switch (this.request.Header.OperationTypes)
                {
                    case (int)OperationType.OperationTypes.ADD:
                        long checkGuid = 0;
                        this.customer = new CustomerRelation
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
                            GENDER = this.request.GENDER
                        };
                        //Add Data to Database
                        checkGuid = customerRelationService.Insert(this.customer);

                        responsedata = new ResponseCustomerRelation
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            CUSTOMER_NAME = this.request.CUSTOMER_NAME,
                            CUSTOMER_SURNAME = this.request.CUSTOMER_SURNAME,
                            CUSTOMER_MIDDLE_NAME = this.request.CUSTOMER_MIDDLE_NAME,
                            RELATION_NUMBER = checkGuid,
                            CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                            IDENTIFICATION_ID = this.request.IDENTIFICATION_ID,
                            EMAIL = this.request.EMAIL,
                            PHONE_NUMBER = this.request.PHONE_NUMBER,
                            GENDER = this.request.GENDER,
                            header = new ResponseHeader
                            {
                                IsSuccess = checkGuid == 0 ? false : true,
                                ResponseCode = checkGuid == 0 ? CommonDefinitions.INTERNAL_SYSTEM_UNKNOWN_ERROR : CommonDefinitions.SUCCESS,
                                ResponseMessage = checkGuid == 0 ? CommonDefinitions.ERROR_MESSAGE : CommonDefinitions.SUCCESS_MESSAGE
                            }
                        };

                        break;
                    case (int)OperationType.OperationTypes.GET:
                        //Get Data
                        this.listcustomerRelation = customerRelationService.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);
                        this.listcustomerRelationResponse = new List<ResponseCustomerRelation>();
                        foreach (var item in this.listcustomerRelationResponse)
                        {
                            responsedata = new ResponseCustomerRelation
                            {
                                INSERT_USER = item.INSERT_USER,
                                UPDATE_USER = item.UPDATE_USER,
                                CUSTOMER_NAME = item.CUSTOMER_NAME,
                                CUSTOMER_SURNAME = item.CUSTOMER_SURNAME,
                                CUSTOMER_MIDDLE_NAME = item.CUSTOMER_MIDDLE_NAME,
                                RELATION_NUMBER = item.RELATION_NUMBER,
                                CUSTOMER_NUMBER = item.CUSTOMER_NUMBER,
                                IDENTIFICATION_ID = item.IDENTIFICATION_ID,
                                EMAIL = item.EMAIL,
                                PHONE_NUMBER = item.PHONE_NUMBER,
                                GENDER = item.GENDER,
                                header = new ResponseHeader
                                {
                                    IsSuccess = true,
                                    ResponseCode = CommonDefinitions.SUCCESS,
                                    ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                                }
                            };
                            this.response.Add(responsedata);
                        }

                        break;
                    case (int)OperationType.OperationTypes.UPDATE:
                        this.listcustomerRelation = customerRelationService.SelectByCustomerNumber(this.request.RELATION_NUMBER);
                        this.customer = new CustomerRelation
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            CUSTOMER_NAME = this.request.CUSTOMER_NAME,
                            CUSTOMER_SURNAME = this.request.CUSTOMER_SURNAME,
                            CUSTOMER_MIDDLE_NAME = this.request.CUSTOMER_MIDDLE_NAME,
                            RELATION_NUMBER = this.request.RELATION_NUMBER,
                            CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                            IDENTIFICATION_ID = this.request.IDENTIFICATION_ID,
                            EMAIL = this.request.EMAIL,
                            PHONE_NUMBER = this.request.PHONE_NUMBER,
                            GENDER = this.request.GENDER,
                        };
                        //Update Customer Information
                        customerRelationService.Update(this.customer);

                        foreach (var item in this.listcustomerRelationResponse)
                        {
                            responsedata = new ResponseCustomerRelation
                            {
                                INSERT_USER = item.INSERT_USER,
                                UPDATE_USER = item.UPDATE_USER,
                                CUSTOMER_NAME = item.CUSTOMER_NAME,
                                CUSTOMER_SURNAME = item.CUSTOMER_SURNAME,
                                CUSTOMER_MIDDLE_NAME = item.CUSTOMER_MIDDLE_NAME,
                                RELATION_NUMBER = item.RELATION_NUMBER,
                                CUSTOMER_NUMBER = item.CUSTOMER_NUMBER,
                                IDENTIFICATION_ID = item.IDENTIFICATION_ID,
                                EMAIL = item.EMAIL,
                                PHONE_NUMBER = item.PHONE_NUMBER,
                                GENDER = item.GENDER,
                                header = new ResponseHeader
                                {
                                    IsSuccess = true,
                                    ResponseCode = CommonDefinitions.SUCCESS,
                                    ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                                }
                            };
                            this.response.Add(responsedata);
                        }

                        break;
                    case (int)OperationType.OperationTypes.DELETE:
                        this.listcustomerRelation = customerRelationService.SelectByCustomerNumber(this.request.RELATION_NUMBER);
                        this.customer = new CustomerRelation
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            RECORD_STATUS = 0,
                            UPDATE_USER = this.request.UPDATE_USER,
                            CUSTOMER_NAME = this.request.CUSTOMER_NAME,
                            CUSTOMER_SURNAME = this.request.CUSTOMER_SURNAME,
                            CUSTOMER_MIDDLE_NAME = this.request.CUSTOMER_MIDDLE_NAME,
                            RELATION_NUMBER = this.request.RELATION_NUMBER,
                            CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                            IDENTIFICATION_ID = this.request.IDENTIFICATION_ID,
                            EMAIL = this.request.EMAIL,
                            PHONE_NUMBER = this.request.PHONE_NUMBER,
                            GENDER = this.request.GENDER
                        };
                        //Update Customer to Passive
                        customerRelationService.Delete(this.customer);

                        foreach (var item in this.listcustomerRelationResponse)
                        {
                            responsedata = new ResponseCustomerRelation
                            {
                                INSERT_USER = item.INSERT_USER,
                                UPDATE_USER = item.UPDATE_USER,
                                CUSTOMER_NAME = item.CUSTOMER_NAME,
                                CUSTOMER_SURNAME = item.CUSTOMER_SURNAME,
                                CUSTOMER_MIDDLE_NAME = item.CUSTOMER_MIDDLE_NAME,
                                RELATION_NUMBER = item.RELATION_NUMBER,
                                CUSTOMER_NUMBER = item.CUSTOMER_NUMBER,
                                IDENTIFICATION_ID = item.IDENTIFICATION_ID,
                                EMAIL = item.EMAIL,
                                PHONE_NUMBER = item.PHONE_NUMBER,
                                GENDER = item.GENDER,
                                header = new ResponseHeader
                                {
                                    IsSuccess = true,
                                    ResponseCode = CommonDefinitions.SUCCESS,
                                    ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                                }
                            };
                            this.response.Add(responsedata);
                        }

                        break;

                    default:
                        break;
                }



            }
            catch (Exception ex)
            {
                string operationError = "HATA:[" + "CustomerNumber:" + this.request.RELATION_NUMBER + ",ResponseCode:" + this.baseResponseMessage.header.ResponseCode + ", ResponseMessage:" + ex.Message + "]";
                log.InfoFormat(operationError, ex);
                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        public override void RollbackOperation()
        {
            throw new NotImplementedException();
        }


    }
}
