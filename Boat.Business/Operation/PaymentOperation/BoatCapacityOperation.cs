using Boat.Backoffice.DataModel.PaymentModule.Entity;
using Boat.Business.Common;
using Boat.Business.Framework;
using Boat.Data;
using Boat.Data.DataModel.BoatModule.Entity;
using Boat.Data.DataModel.BoatModule.Service.Interface;
using Boat.Data.DataModel.PaymentModule.Service;
using Boat.Data.DataModel.PaymentModule.Service.Interface;
using Boat.Data.Dto;
using Boat.Data.Dto.PaymentModule.Request;
using Boat.Data.Dto.PaymentModule.Response;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Boat.Business.Operation.PaymentOperation
{
    public class BoatCapacityOperation : OperationBase
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IBoatsCapacityService boatsCapacityService;
        private IReservationService reservationService;
        private IBoatsService boatsService;

        public RequestBoatCapacity request = new RequestBoatCapacity();
        public ResponseBoatCapacity response = new ResponseBoatCapacity();
        public BoatsCapacity boatCapacity = null;
        public Reservation reservation = null;
        public BaseResponseMessage baseResponseMessage = null;
        private bool checkValue = false;

        public BoatCapacityOperation(RequestBoatCapacity request, BoatsCapacityService service)
        {
            this.request.Header = new Header();
            this.request = request;
            this.boatsCapacityService = service;
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
            else if (this.request.RESERVATION_ID == 0)
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.RESERVATION_ID_NOT_FOUND;
            }
            else if (String.IsNullOrEmpty(this.request.Header.TokenId))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.TOKEN_NOT_VALID;
            }

            reservation = reservationService.SelectByReservationId(this.request.RESERVATION_ID);

            if (!Tokenizer.checkToken(this.request.Header.TokenId, reservation.CUSTOMER_NUMBER))
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
            //Validate Reques Header / Constants
            this.baseResponseMessage = ValidateInput();
            if (!this.baseResponseMessage.header.IsSuccess)
                throw new Exception(this.baseResponseMessage.header.ResponseMessage);

            switch (this.request.Header.OperationTypes)
            {
                case (int)OperationType.OperationTypes.ADD:
                    #region ADD
                    Int32 capacity = 0;
                    Boats boat = new Boats();
                    boat = boatsService.SelectByBoatId(request.BOAT_ID);
                    if (Convert.ToInt32(request.CAPACITY) < boat.QUANTITY)
                    {
                        capacity = boat.QUANTITY - Convert.ToInt32(request.CAPACITY);
                    }
                    else
                        throw new Exception(CommonDefinitions.BOAT_CAPACITY_IS_NOT_ENOUGH);

                    boatCapacity = new BoatsCapacity
                    {
                        BOAT_ID = request.BOAT_ID,
                        CAPACITY = request.CAPACITY,
                        RESERVATION_DATE = request.RESERVATION_DATE,
                        RESERVATION_END_DATE = request.RESERVATION_END_DATE,
                        RESERVATION_ID = request.RESERVATION_ID
                    };

                    BoatsCapacity responseBoatsCapacity = new BoatsCapacity();
                    responseBoatsCapacity = boatsCapacityService.SelectByBoatId(boatCapacity);
                    if (responseBoatsCapacity == null)
                    {
                        boatCapacity.CAPACITY = capacity.ToString();

                        boatCapacity.BOAT_CAPACITY_ID = boatsCapacityService.Insert(boatCapacity);
                    }
                    else
                    {
                        responseBoatsCapacity.RESERVATION_ID = boatCapacity.RESERVATION_ID;
                        Int32 newCapactiy = Convert.ToInt32(response.CAPACITY) - Convert.ToInt32(boatCapacity.CAPACITY);
                        if (newCapactiy < 0)
                            throw new Exception(CommonDefinitions.BOAT_CAPACITY_IS_NOT_ENOUGH);
                        else
                        {
                            response.CAPACITY = newCapactiy.ToString();
                            boatsCapacityService.Update(responseBoatsCapacity);
                        }

                    }

                    this.response = new ResponseBoatCapacity
                    {
                        BOAT_ID = responseBoatsCapacity.BOAT_ID,
                        CAPACITY = responseBoatsCapacity.CAPACITY,
                        RESERVATION_DATE = responseBoatsCapacity.RESERVATION_DATE,
                        RESERVATION_END_DATE = responseBoatsCapacity.RESERVATION_END_DATE,
                        RESERVATION_ID = responseBoatsCapacity.RESERVATION_ID,
                        BOAT_CAPACITY_ID = responseBoatsCapacity.BOAT_CAPACITY_ID,
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
                    #region Get
                    boatCapacity = new BoatsCapacity
                    {
                        BOAT_ID = request.BOAT_ID,
                        CAPACITY = request.CAPACITY,
                        RESERVATION_DATE = request.RESERVATION_DATE,
                        RESERVATION_END_DATE = request.RESERVATION_END_DATE,
                        RESERVATION_ID = request.RESERVATION_ID
                    };

                    BoatsCapacity responseCapacity = new BoatsCapacity();
                    responseCapacity = boatsCapacityService.SelectByBoatId(boatCapacity);

                    this.response = new ResponseBoatCapacity
                    {
                        BOAT_ID = responseCapacity.BOAT_ID,
                        CAPACITY = responseCapacity.CAPACITY,
                        RESERVATION_DATE = responseCapacity.RESERVATION_DATE,
                        RESERVATION_END_DATE = responseCapacity.RESERVATION_END_DATE,
                        RESERVATION_ID = responseCapacity.RESERVATION_ID,
                        BOAT_CAPACITY_ID = responseCapacity.BOAT_CAPACITY_ID,
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

        public override void RollbackOperation()
        {
            throw new NotImplementedException();
        }
    }
}
