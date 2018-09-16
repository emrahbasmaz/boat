using Boat.Business.Common;
using Boat.Business.Framework;
using Boat.Data;
using Boat.Data.DataModel.BoatModule.Entity;
using Boat.Data.DataModel.BoatModule.Service;
using Boat.Data.DataModel.BoatModule.Service.Interface;
using Boat.Data.DataModel.GeneralModule.Service.Interface;
using Boat.Data.Dto;
using Boat.Data.Dto.BoatModule.Request;
using Boat.Data.Dto.BoatModule.Response;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Enums = Boat.Business.Framework.Enums;

namespace Boat.Business.Operation.MerchantOperation
{
    public class BoatOperation : OperationBase
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IFavoritesServices favoritesServices;
        private IBoatsService boatsService;
        private IRegionService regionService;

        public RequestBoats request = new RequestBoats();
        public ResponseBoats response = new ResponseBoats();
        public BaseResponseMessage baseResponseMessage = null;

        Boats boat = null;
        List<Boats> listboat = null;
        List<ResponseBoats> reponseAllBoats = new List<ResponseBoats>();

        public BoatOperation(RequestBoats request, BoatsService service)
        {
            this.request.Header = new Header();
            this.request = request;
            this.boatsService = service;

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
            else if (String.IsNullOrEmpty(this.request.Header.TokenId))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.TOKEN_NOT_VALID;
            }
            else if (!Tokenizer.checkToken(this.request.Header.TokenId, this.request.CAPTAIN_ID))
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

                switch (this.request.Header.OperationTypes)
                {
                    case (int)OperationType.OperationTypes.ADD:
                        #region Add
                        long checkGuid = 0;
                        this.boat = new Boats
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            BOAT_INFO = this.request.BOAT_INFO,
                            BOAT_NAME = this.request.BOAT_NAME,
                            CAPTAIN_ID = this.request.CAPTAIN_ID,
                            FLAG = this.request.FLAG,
                            QUANTITY = this.request.QUANTITY,
                            ROTA_INFO = this.request.ROTA_INFO,
                            REGION_ID = this.request.REGION_ID,
                            PRICE = this.request.PRICE,
                            PRIVATE_PRICE = this.request.PRIVATE_PRICE,
                            TOUR_TYPE = this.request.TOUR_TYPE
                        };

                        checkGuid = boatsService.Insert(this.boat);

                        this.response = new ResponseBoats
                        {
                            INSERT_USER = this.boat.INSERT_USER,
                            UPDATE_USER = this.boat.UPDATE_USER,
                            BOAT_ID = this.boat.BOAT_ID,
                            BOAT_INFO = this.boat.BOAT_INFO,
                            BOAT_NAME = this.boat.BOAT_NAME,
                            CAPTAIN_ID = this.request.CAPTAIN_ID,
                            FLAG = this.request.FLAG,
                            QUANTITY = this.request.QUANTITY,
                            ROTA_INFO = this.request.ROTA_INFO,
                            REGION_ID = this.request.REGION_ID,
                            REGION_NAME = CommonServices.GetRegionName(this.request.REGION_ID),
                            PRICE = this.request.PRICE,
                            PRIVATE_PRICE = this.request.PRIVATE_PRICE,
                            TOUR_TYPE = this.request.TOUR_TYPE,
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
                        //Bölgesel Arama
                        if (this.request.REGION_ID != 0)
                        {
                            this.listboat = boatsService.SelectByRegionId(this.request.REGION_ID);
                            if (listboat != null && listboat.Count > 0)
                            {
                                foreach (var item in listboat)
                                {
                                    this.response = new ResponseBoats
                                    {
                                        INSERT_USER = item.INSERT_USER,
                                        UPDATE_USER = item.UPDATE_USER,
                                        BOAT_ID = item.BOAT_ID,
                                        BOAT_INFO = item.BOAT_INFO,
                                        BOAT_NAME = item.BOAT_NAME,
                                        CAPTAIN_ID = item.CAPTAIN_ID,
                                        FLAG = item.FLAG,
                                        QUANTITY = item.QUANTITY,
                                        ROTA_INFO = item.ROTA_INFO,
                                        REGION_ID = item.REGION_ID,
                                        REGION_NAME = CommonServices.GetRegionName(this.request.REGION_ID),
                                        header = new ResponseHeader
                                        {
                                            IsSuccess = true,
                                            ResponseCode = CommonDefinitions.SUCCESS,
                                            ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                                        }

                                    };
                                    reponseAllBoats.Add(this.response);
                                }
                                this.response.AllBoats = reponseAllBoats;
                            }
                        }
                        else
                        {
                            //Tekli Arama
                            this.boat = boatsService.SelectByBoatId(this.request.BOAT_ID);

                            this.response = new ResponseBoats
                            {
                                INSERT_USER = this.boat.INSERT_USER,
                                UPDATE_USER = this.boat.UPDATE_USER,
                                BOAT_ID = this.boat.BOAT_ID,
                                BOAT_INFO = this.boat.BOAT_INFO,
                                BOAT_NAME = this.boat.BOAT_NAME,
                                CAPTAIN_ID = this.boat.CAPTAIN_ID,
                                FLAG = this.boat.FLAG,
                                QUANTITY = this.boat.QUANTITY,
                                ROTA_INFO = this.boat.ROTA_INFO,
                                REGION_ID = this.boat.REGION_ID,
                                REGION_NAME = CommonServices.GetRegionName(this.request.REGION_ID),
                                header = new ResponseHeader
                                {
                                    IsSuccess = true,
                                    ResponseCode = CommonDefinitions.SUCCESS,
                                    ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                                }

                            };

                        }
                        #endregion
                        break;
                    case (int)OperationType.OperationTypes.UPDATE:
                        #region  Update
                        this.boat = new Boats
                        {
                            BOAT_ID = this.request.BOAT_ID,
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            BOAT_INFO = this.request.BOAT_INFO,
                            BOAT_NAME = this.request.BOAT_NAME,
                            CAPTAIN_ID = this.request.CAPTAIN_ID,
                            FLAG = this.request.FLAG,
                            QUANTITY = this.request.QUANTITY,
                            ROTA_INFO = this.request.ROTA_INFO,
                            REGION_ID = this.request.REGION_ID,
                            PRICE = this.request.PRICE,
                            PRIVATE_PRICE = this.request.PRIVATE_PRICE,
                            TOUR_TYPE = this.request.TOUR_TYPE
                        };

                        boatsService.Update(this.boat);

                        this.response = new ResponseBoats
                        {
                            INSERT_USER = this.boat.INSERT_USER,
                            UPDATE_USER = this.boat.UPDATE_USER,
                            BOAT_ID = this.boat.BOAT_ID,
                            BOAT_INFO = this.boat.BOAT_INFO,
                            BOAT_NAME = this.boat.BOAT_NAME,
                            CAPTAIN_ID = this.boat.CAPTAIN_ID,
                            FLAG = this.boat.FLAG,
                            QUANTITY = this.boat.QUANTITY,
                            ROTA_INFO = this.boat.ROTA_INFO,
                            REGION_ID = this.boat.REGION_ID,
                            REGION_NAME = CommonServices.GetRegionName(this.request.REGION_ID),
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
                        this.boat = new Boats
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            BOAT_INFO = this.request.BOAT_INFO,
                            BOAT_NAME = this.request.BOAT_NAME,
                            CAPTAIN_ID = this.request.CAPTAIN_ID,
                            FLAG = this.request.FLAG,
                            QUANTITY = this.request.QUANTITY,
                            ROTA_INFO = this.request.ROTA_INFO,
                            REGION_ID = this.request.REGION_ID
                        };

                        boatsService.Delete(this.boat);
                        this.response = new ResponseBoats
                        {
                            INSERT_USER = this.boat.INSERT_USER,
                            UPDATE_USER = this.boat.UPDATE_USER,
                            BOAT_ID = this.boat.BOAT_ID,
                            BOAT_INFO = this.boat.BOAT_INFO,
                            BOAT_NAME = this.boat.BOAT_NAME,
                            CAPTAIN_ID = this.boat.CAPTAIN_ID,
                            FLAG = this.boat.FLAG,
                            QUANTITY = this.boat.QUANTITY,
                            ROTA_INFO = this.boat.ROTA_INFO,
                            REGION_ID = this.boat.REGION_ID,
                            REGION_NAME = CommonServices.GetRegionName(this.request.REGION_ID),
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
                string operationError = "HATA:[" + "BoatId:" + this.request.BOAT_ID + ",ResponseCode:" + this.baseResponseMessage.header.ResponseCode + ", ResponseMessage:" + ex.Message + "]";
                log.InfoFormat(operationError, ex);
                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        public override void RollbackOperation()
        {
            TranSeq = Enums.TransactionSequence.ROLLBACK;
            throw new NotImplementedException();
        }

    }
}
