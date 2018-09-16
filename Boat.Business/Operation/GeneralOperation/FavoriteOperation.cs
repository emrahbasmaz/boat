using Boat.Business.Common;
using Boat.Business.Framework;
using Boat.Data;
using Boat.Data.DataModel.BoatModule.Entity;
using Boat.Data.DataModel.BoatModule.Service.Interface;
using Boat.Data.DataModel.GeneralModule.Entity;
using Boat.Data.DataModel.GeneralModule.Service;
using Boat.Data.DataModel.GeneralModule.Service.Interface;
using Boat.Data.Dto;
using Boat.Data.Dto.BoatModule.Response;
using Boat.Data.Dto.GeneralModule.Request;
using Boat.Data.Dto.GeneralModule.Response;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Boat.Business.Operation.GeneralOperation
{
    public class FavoriteOperation : OperationBase
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RequestFavorites request = new RequestFavorites();
        public ResponseFavorites response = new ResponseFavorites();
        public BaseResponseMessage baseResponseMessage = null;
        private IFavoritesServices favoritesServices;
        private IBoatsService boatsService;
        private IRegionService regionService;

        Favorites favorite = null;
        List<Favorites> listfavorites = null;
        List<ResponseFavorites> responseAllFavorite = new List<ResponseFavorites>();

        Boats boat = null;
        List<Boats> listboat = null;
        ResponseBoats responseBoats = null;
        List<ResponseBoats> reponseAllBoats = new List<ResponseBoats>();

        Region region = null;
        List<Region> listregion = null;
        public FavoriteOperation(RequestFavorites request, FavoritesServices services)
        {
            this.request.Header = new Data.Dto.Header();
            this.request = request;
            this.favoritesServices = services;
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
            else if (this.request.CUSTOMER_NUMBER == 0)
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.CUSTOMER_NOT_FOUND;
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
                        #region ADD
                        long checkGuid = 0;
                        this.favorite = new Favorites
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                            BOAT_ID = this.request.BOAT_ID
                        };

                        checkGuid = favoritesServices.Insert(this.favorite);
                        this.response = new ResponseFavorites
                        {
                            BOAT_ID = this.request.BOAT_ID,
                            CUSTOMER_NUMBER = this.favorite.CUSTOMER_NUMBER,
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
                        listfavorites = favoritesServices.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);
                        //Take all favorites for CustomerNumber
                        foreach (var item in listfavorites)
                        {
                            this.response = new ResponseFavorites
                            {
                                CUSTOMER_NUMBER = item.CUSTOMER_NUMBER,
                                BOAT_ID = item.BOAT_ID,
                                header = new ResponseHeader
                                {
                                    IsSuccess = true,
                                    ResponseCode = CommonDefinitions.SUCCESS,
                                    ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                                }
                            };
                            responseAllFavorite.Add(this.response);
                        }

                        //Take All Boat Info for Favorites
                        this.listboat = boatsService.SelectAllBoat();
                        this.listregion = regionService.SelectAllRegion();
                        foreach (var item in responseAllFavorite)
                        {

                            this.boat = this.listboat.Where(s => s.BOAT_ID == item.BOAT_ID).FirstOrDefault();
                            this.responseBoats = new ResponseBoats
                            {
                                BOAT_ID = this.boat.BOAT_ID,
                                BOAT_INFO = this.boat.BOAT_INFO,
                                BOAT_NAME = this.boat.BOAT_NAME,
                                CAPTAIN_ID = this.boat.CAPTAIN_ID,
                                FLAG = this.boat.FLAG,
                                QUANTITY = this.boat.QUANTITY,
                                REGION_ID = this.boat.REGION_ID,
                                ROTA_INFO = this.boat.ROTA_INFO,
                                REGION_NAME = Enum.GetName(typeof(Framework.Enums.Region), this.boat.REGION_ID),
                                header = new ResponseHeader
                                {
                                    IsSuccess = true,
                                    ResponseCode = CommonDefinitions.SUCCESS,
                                    ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                                }
                            };

                            reponseAllBoats.Add(this.responseBoats);
                        }

                        #endregion
                        break;
                    case (int)OperationType.OperationTypes.DELETE:
                        #region DELETE
                        if (!this.request.IsDeleteByCustomerNumber)
                        {
                            this.favorite = new Favorites
                            {
                                INSERT_USER = this.request.INSERT_USER,
                                UPDATE_USER = this.request.UPDATE_USER,
                                CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                                BOAT_ID = this.request.BOAT_ID
                            };

                            if (favoritesServices.Delete(this.favorite))
                            {
                                this.response = new ResponseFavorites
                                {
                                    CUSTOMER_NUMBER = this.favorite.CUSTOMER_NUMBER,
                                    BOAT_ID = this.favorite.BOAT_ID,
                                    header = new ResponseHeader
                                    {
                                        IsSuccess = true,
                                        ResponseCode = CommonDefinitions.SUCCESS,
                                        ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                                    }
                                };
                            }
                        }
                        else
                        {
                            //Delete all Favorites Boats for customer
                            if (favoritesServices.DeleteAllforCustomer(this.favorite.CUSTOMER_NUMBER))
                            {
                                this.response = new ResponseFavorites
                                {
                                    CUSTOMER_NUMBER = this.favorite.CUSTOMER_NUMBER,
                                    BOAT_ID = this.favorite.BOAT_ID,
                                    header = new ResponseHeader
                                    {
                                        IsSuccess = true,
                                        ResponseCode = CommonDefinitions.SUCCESS,
                                        ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                                    }
                                };
                            }
                        }

                        #endregion
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
    }
}
