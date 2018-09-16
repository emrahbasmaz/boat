using Boat.Backoffice.Common;
using Boat.Backoffice.DataModel.BoatModule.Entity;
using Boat.Backoffice.DataModel.BoatModule.RequestMessages;
using Boat.Backoffice.DataModel.BoatModule.ResponseMessages;
using Boat.Backoffice.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Boat.Backoffice.Controller.MerchantController
{
    public class CaptainOperation : OperationBase
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RequestCaptain request = new RequestCaptain();
        public Captains captain = null;
        public ResponseCaptain response = null;
        public BaseResponseMessage baseResponseMessage = null;
        public CaptainOperation(RequestCaptain request)
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
            else if (String.IsNullOrEmpty(this.request.PHONE_NUMBER))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.PHONE_NUMBER_NOT_FOUND;
            }
            else if (String.IsNullOrEmpty(this.request.CAPTAIN_NAME))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.INVALID_NAME;
            }
            else if (String.IsNullOrEmpty(this.request.CAPTAIN_SURNAME))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.INVALID_NAME;
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
                        #region ADD
                        long checkGuid = 0;
                        this.captain = new Captains
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            CAPTAIN_NAME = this.request.CAPTAIN_NAME,
                            CAPTAIN_MIDDLE_NAME = this.request.CAPTAIN_MIDDLE_NAME,
                            CAPTAIN_SURNAME = this.request.CAPTAIN_SURNAME,
                            BOAT_ID = this.request.BOAT_ID,
                            IDENTIFICATION_ID = this.request.IDENTIFICATION_ID,
                            EMAIL = this.request.EMAIL,
                            PHONE_NUMBER = this.request.PHONE_NUMBER,
                            CAPTAIN_INFO = this.request.CAPTAIN_INFO
                        };
                        //Add Data to Database
                        checkGuid = Captains.Insert(this.captain);

                        //Connect captain to related Boat
                        Boats boat = new Boats();
                        boat = Boats.SelectByBoatId(this.captain.BOAT_ID);
                        boat.CAPTAIN_ID = checkGuid;
                        Boats.Update(boat);

                        this.response = new ResponseCaptain
                        {
                            CAPTAIN_ID = checkGuid,
                            CAPTAIN_NAME = this.request.CAPTAIN_NAME,
                            CAPTAIN_MIDDLE_NAME = this.request.CAPTAIN_MIDDLE_NAME,
                            CAPTAIN_SURNAME = this.request.CAPTAIN_SURNAME,
                            BOAT_ID = this.request.BOAT_ID,
                            IDENTIFICATION_ID = this.request.IDENTIFICATION_ID,
                            EMAIL = this.request.EMAIL,
                            PHONE_NUMBER = this.request.PHONE_NUMBER,
                            CAPTAIN_INFO = this.request.CAPTAIN_INFO,
                            header = new ResponseHeader
                            {
                                IsSuccess = checkGuid == 0 ? false : true,
                                ResponseCode = checkGuid == 0 ? CommonDefinitions.INTERNAL_SYSTEM_UNKNOWN_ERROR : CommonDefinitions.SUCCESS,
                                ResponseMessage = checkGuid == 0 ? CommonDefinitions.ERROR_MESSAGE : CommonDefinitions.SUCCESS_MESSAGE
                            }

                        };
                        #endregion
                        break;
                    case (int)OperationType.OperationTypes.UPDATE:
                        #region UPDATE
                        this.captain = new Captains
                        {
                            UPDATE_USER = this.request.UPDATE_USER,
                            CAPTAIN_NAME = this.request.CAPTAIN_NAME,
                            CAPTAIN_MIDDLE_NAME = this.request.CAPTAIN_MIDDLE_NAME,
                            CAPTAIN_SURNAME = this.request.CAPTAIN_SURNAME,
                            BOAT_ID = this.request.BOAT_ID,
                            IDENTIFICATION_ID = this.request.IDENTIFICATION_ID,
                            EMAIL = this.request.EMAIL,
                            PHONE_NUMBER = this.request.PHONE_NUMBER,
                            CAPTAIN_INFO = this.request.CAPTAIN_INFO
                        };
                        //Modify Data to Database
                        Captains.Update(this.captain);
                        this.response = new ResponseCaptain
                        {
                            CAPTAIN_NAME = this.request.CAPTAIN_NAME,
                            CAPTAIN_MIDDLE_NAME = this.request.CAPTAIN_MIDDLE_NAME,
                            CAPTAIN_SURNAME = this.request.CAPTAIN_SURNAME,
                            BOAT_ID = this.request.BOAT_ID,
                            IDENTIFICATION_ID = this.request.IDENTIFICATION_ID,
                            EMAIL = this.request.EMAIL,
                            PHONE_NUMBER = this.request.PHONE_NUMBER,
                            CAPTAIN_INFO = this.request.CAPTAIN_INFO,
                            header = new ResponseHeader
                            {
                                IsSuccess = true,
                                ResponseCode = CommonDefinitions.SUCCESS,
                                ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                            }

                        };
                        #endregion
                        break;
                    case (int)OperationType.OperationTypes.GET:
                        #region GET
                        //Get Data
                        this.captain = Captains.SelectByBoatId(this.request.BOAT_ID);

                        this.response = new ResponseCaptain
                        {
                            CAPTAIN_NAME = this.captain.CAPTAIN_NAME,
                            CAPTAIN_MIDDLE_NAME = this.captain.CAPTAIN_MIDDLE_NAME,
                            CAPTAIN_SURNAME = this.captain.CAPTAIN_SURNAME,
                            BOAT_ID = this.captain.BOAT_ID,
                            IDENTIFICATION_ID = this.captain.IDENTIFICATION_ID,
                            EMAIL = this.captain.EMAIL,
                            PHONE_NUMBER = this.captain.PHONE_NUMBER,
                            CAPTAIN_INFO = this.captain.CAPTAIN_INFO,
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
                        this.captain = Captains.SelectByBoatId(this.request.BOAT_ID);

                        Captains.Delete(this.captain);
                        this.response = new ResponseCaptain
                        {
                            CAPTAIN_NAME = "",
                            CAPTAIN_MIDDLE_NAME = "",
                            CAPTAIN_SURNAME = "",
                            BOAT_ID = this.captain.BOAT_ID,
                            IDENTIFICATION_ID = 0,
                            EMAIL = "",
                            PHONE_NUMBER = "",
                            CAPTAIN_INFO = "",
                            header = new ResponseHeader
                            {
                                IsSuccess = true,
                                ResponseCode = CommonDefinitions.SUCCESS,
                                ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                            }

                        };

                        //DisConnect captain to related Boat
                        Boats boats = new Boats();
                        boats = Boats.SelectByBoatId(this.captain.BOAT_ID);
                        boats.CAPTAIN_ID = 0;
                        Boats.Update(boats);

                        #endregion
                        break;


                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                string operationError = "HATA:[" + "CaptainId:" + this.request.CAPTAIN_ID + ",ResponseCode:" + this.baseResponseMessage.header.ResponseCode + ", ResponseMessage:" + ex.Message + "]";
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
