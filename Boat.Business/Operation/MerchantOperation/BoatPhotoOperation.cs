using Boat.Business.Framework;
using Boat.Data;
using Boat.Data.DataModel.BoatModule.Entity;
using Boat.Data.DataModel.BoatModule.Service;
using Boat.Data.DataModel.BoatModule.Service.Interface;
using Boat.Data.Dto;
using Boat.Data.Dto.BoatModule.Request;
using Boat.Data.Dto.BoatModule.Response;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Boat.Business.Operation.MerchantOperation
{
    public class BoatPhotoOperation : OperationBase
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IBoatPhotosService boatPhotosService;

        public RequestBoatPhoto request = null;
        public ResponseBoatPhoto response = null;
        public List<ResponseBoatPhoto> listResponseBoatPhoto = null;
        public List<BoatPhotos> listresponse = null;
        public BaseResponseMessage baseResponseMessage = null;
        public BoatPhotos photos = null;

        public BoatPhotoOperation(RequestBoatPhoto request, BoatPhotosService service)
        {
            this.request.Header = new Header();
            this.request = request;
            this.boatPhotosService = service;
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
            else if (this.request.BOAT_ID == 0)
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.BOAT_NOT_FOUND;
            }
            else if (String.IsNullOrEmpty(this.request.PHOTO))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.BOAT_PHOTOS_NOT_FOUND;
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
                        this.photos = new BoatPhotos
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            BOAT_ID = this.request.BOAT_ID,
                            PHOTO = this.request.PHOTO
                        };
                        //Add Data to Database
                        checkGuid = boatPhotosService.Insert(this.photos);

                        this.response = new ResponseBoatPhoto
                        {
                            PHOTO_ID = checkGuid,
                            PHOTO = this.request.PHOTO,
                            BOAT_ID = this.request.BOAT_ID,
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
                        this.photos = new BoatPhotos
                        {
                            UPDATE_USER = this.request.UPDATE_USER,
                            BOAT_ID = this.request.BOAT_ID,
                            PHOTO = this.request.PHOTO
                        };
                        boatPhotosService.Update(this.photos);
                        this.response = new ResponseBoatPhoto
                        {
                            PHOTO = this.request.PHOTO,
                            BOAT_ID = this.request.BOAT_ID,
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
                        listResponseBoatPhoto = new List<ResponseBoatPhoto>();
                        listresponse = boatPhotosService.SelectByBoatId(this.request.BOAT_ID);
                        foreach (var item in listresponse)
                        {
                            this.response = new ResponseBoatPhoto
                            {
                                BOAT_ID = item.BOAT_ID,
                                PHOTO = item.PHOTO,
                                PHOTO_ID = item.PHOTO_ID,
                                header = new ResponseHeader
                                {
                                    IsSuccess = true,
                                    ResponseCode = CommonDefinitions.SUCCESS,
                                    ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                                }
                            };
                            listResponseBoatPhoto.Add(this.response);
                        }
                        #endregion
                        break;
                    case (int)OperationType.OperationTypes.DELETE:
                        #region DELETE
                        this.photos = new BoatPhotos
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            BOAT_ID = this.request.BOAT_ID,
                            PHOTO = this.request.PHOTO
                        };
                        boatPhotosService.Delete(this.photos);
                        this.response = new ResponseBoatPhoto
                        {
                            BOAT_ID = 0,
                            PHOTO = "",
                            PHOTO_ID = 0,
                            header = new ResponseHeader
                            {
                                IsSuccess = true,
                                ResponseCode = CommonDefinitions.SUCCESS,
                                ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                            }
                        };
                        #endregion
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                string operationError = "HATA:[" + "BOAT_ID:" + this.request.BOAT_ID + ",ResponseCode:" + this.baseResponseMessage.header.ResponseCode + ", ResponseMessage:" + ex.Message + "]";
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
