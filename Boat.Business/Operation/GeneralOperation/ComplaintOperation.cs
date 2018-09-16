using Boat.Business.Framework;
using Boat.Data;
using Boat.Data.DataModel.GeneralModule.Entity;
using Boat.Data.DataModel.GeneralModule.Service;
using Boat.Data.DataModel.GeneralModule.Service.Interface;
using Boat.Data.Dto;
using Boat.Data.Dto.GeneralModule.Request;
using Boat.Data.Dto.GeneralModule.Response;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Boat.Business.Operation.GeneralOperation
{
    public class ComplaintOperation : OperationBase
    {

        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IComplaintsService complaintService;
        public Complaints complaint = null;
        public RequestComplaints request = new RequestComplaints();
        public ResponseComplaints response = null;

        public BaseResponseMessage baseResponseMessage = null;

        public ComplaintOperation(RequestComplaints request, ComplaintsService service)
        {
            this.request.Header = new Header();
            this.request = request;
            this.complaintService = service;
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

            switch (this.request.Header.OperationTypes)
            {
                case (int)OperationType.OperationTypes.ADD:
                    long checkGuid = 0;
                    this.complaint = new Complaints
                    {
                        INSERT_USER = this.request.INSERT_USER,
                        UPDATE_USER = this.request.UPDATE_USER,
                        CONTENT_HEADER = this.request.CONTENT_HEADER,
                        CONTENT_TEXT = this.request.CONTENT_TEXT,
                        CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                        RESERVATION_ID = this.request.RESERVATION_ID,
                        EMAIL = this.request.EMAIL,
                        PHONE_NUMBER = this.request.PHONE_NUMBER,
                        PHOTO = this.request.PHOTO,
                        CONFIRM = this.request.CONFIRM
                    };
                    //Add Data to Database
                    checkGuid = this.complaintService.Insert(this.complaint);

                    response = new ResponseComplaints
                    {
                        INSERT_USER = this.request.INSERT_USER,
                        UPDATE_USER = this.request.UPDATE_USER,
                        COMPLAINT_ID = checkGuid,
                        CONTENT_HEADER = this.request.CONTENT_HEADER,
                        CONTENT_TEXT = this.request.CONTENT_TEXT,
                        CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                        RESERVATION_ID = this.request.RESERVATION_ID,
                        EMAIL = this.request.EMAIL,
                        PHONE_NUMBER = this.request.PHONE_NUMBER,
                        PHOTO = this.request.PHOTO,
                        CONFIRM = this.request.CONFIRM,
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

                    this.complaint = this.complaintService.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);
                    response = new ResponseComplaints
                    {
                        CONTENT_HEADER = this.request.CONTENT_HEADER,
                        CONTENT_TEXT = this.request.CONTENT_TEXT,
                        CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                        RESERVATION_ID = this.request.RESERVATION_ID,
                        EMAIL = this.request.EMAIL,
                        PHONE_NUMBER = this.request.PHONE_NUMBER,
                        PHOTO = this.request.PHOTO,
                        CONFIRM = this.request.CONFIRM,
                        header = new ResponseHeader
                        {
                            IsSuccess = true,
                            ResponseCode = CommonDefinitions.SUCCESS,
                            ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                        }
                    };
                    break;
                case (int)OperationType.OperationTypes.UPDATE:
                    this.complaint = this.complaintService.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);
                    this.complaint = new Complaints
                    {
                        INSERT_USER = this.request.INSERT_USER,
                        UPDATE_USER = this.request.UPDATE_USER,
                        CONTENT_HEADER = this.request.CONTENT_HEADER,
                        CONTENT_TEXT = this.request.CONTENT_TEXT,
                        CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                        RESERVATION_ID = this.request.RESERVATION_ID,
                        EMAIL = this.request.EMAIL,
                        PHONE_NUMBER = this.request.PHONE_NUMBER,
                        PHOTO = this.request.PHOTO,
                        CONFIRM = this.request.CONFIRM,
                    };
                    //Update Customer Information
                    this.complaintService.Update(this.complaint);

                    response = new ResponseComplaints
                    {
                        CONTENT_HEADER = this.request.CONTENT_HEADER,
                        CONTENT_TEXT = this.request.CONTENT_TEXT,
                        CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                        RESERVATION_ID = this.request.RESERVATION_ID,
                        EMAIL = this.request.EMAIL,
                        PHONE_NUMBER = this.request.PHONE_NUMBER,
                        PHOTO = this.request.PHOTO,
                        CONFIRM = this.request.CONFIRM,
                        header = new ResponseHeader
                        {
                            IsSuccess = true,
                            ResponseCode = CommonDefinitions.SUCCESS,
                            ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                        }
                    };

                    break;
                case (int)OperationType.OperationTypes.DELETE:
                    this.complaint = new Complaints
                    {
                        INSERT_USER = this.request.INSERT_USER,
                        UPDATE_USER = this.request.UPDATE_USER,
                        CONTENT_HEADER = this.request.CONTENT_HEADER,
                        CONTENT_TEXT = this.request.CONTENT_TEXT,
                        CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                        RESERVATION_ID = this.request.RESERVATION_ID,
                        EMAIL = this.request.EMAIL,
                        PHONE_NUMBER = this.request.PHONE_NUMBER,
                        PHOTO = this.request.PHOTO,
                        CONFIRM = this.request.CONFIRM
                    };

                    this.complaint = this.complaintService.Delete(this.complaint);
                    response = new ResponseComplaints
                    {
                        CONTENT_HEADER = this.request.CONTENT_HEADER,
                        CONTENT_TEXT = this.request.CONTENT_TEXT,
                        CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                        RESERVATION_ID = this.request.RESERVATION_ID,
                        EMAIL = this.request.EMAIL,
                        PHONE_NUMBER = this.request.PHONE_NUMBER,
                        PHOTO = this.request.PHOTO,
                        CONFIRM = this.request.CONFIRM,
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

        public override void RollbackOperation()
        {
            throw new NotImplementedException();
        }

    }
}
