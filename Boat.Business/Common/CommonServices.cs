using Boat.Backoffice.DataModel.PaymentModule.Entity;
using Boat.Business.Framework;
using Boat.Business.Operation.PaymentOperation;
using Boat.Data;
using Boat.Data.DataModel.BoatModule.Entity;
using Boat.Data.DataModel.BoatModule.Service.Interface;
using Boat.Data.DataModel.CustomerModule.Entity;
using Boat.Data.DataModel.CustomerModule.Service;
using Boat.Data.DataModel.CustomerModule.Service.Interface;
using Boat.Data.DataModel.GeneralModule.Entity;
using Boat.Data.DataModel.GeneralModule.Service;
using Boat.Data.DataModel.GeneralModule.Service.Interface;
using Boat.Data.DataModel.PaymentModule.Service;
using Boat.Data.DataModel.PaymentModule.Service.Interface;
using Boat.Data.Dto;
using Boat.Data.Dto.BoatModule.Request;
using Boat.Data.Dto.BoatModule.Response;
using Boat.Data.Dto.CustomerModule.Response;
using Boat.Data.Dto.GeneralModule.Response;
using Boat.Data.Dto.PaymentModule.Request;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace Boat.Business.Common
{
    public class CommonServices
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static ICustomerService customerServices;
        private static ICustomerAddressService customerAddressService;
        private static IBoatsService boatsService;
        private static ICaptainsService captainsService;
        private static IRegionService regionService;
        private static IPaymentTransactionService paymentTransactionService;
        private static IReservationService reservationService;
        private static IFavoritesServices favoritesServices;

        public static ResponseCustomerAddress GetCustomerAddress(long customerNumber)
        {
            try
            {
                CustomerAddress customerAddress = customerAddressService.SelectByCustomerNumber(customerNumber);
                ResponseCustomerAddress response = new ResponseCustomerAddress
                {
                    CITY = customerAddress.CITY,
                    COUNTRY = customerAddress.COUNTRY,
                    CUSTOMER_NUMBER = customerAddress.CUSTOMER_NUMBER,
                    DESCRIPTION = customerAddress.DESCRIPTION,
                    ZIPCODE = customerAddress.ZIPCODE,
                    header = new ResponseHeader
                    {
                        IsSuccess = true,
                        ResponseCode = CommonDefinitions.SUCCESS,
                        ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                    }
                };

                return response;
            }
            catch (Exception ex)
            {
                log.Error("An unexpected error occured while getting customer address informations");
                throw ex;
            }
        }

        public static ResponsePersonalInformation GetCustomer(long customerNumber)
        {
            try
            {
                Customer customer = customerServices.SelectByCustomerNumber(customerNumber);
                ResponsePersonalInformation response = new ResponsePersonalInformation
                {
                    CUSTOMER_NAME = customer.CUSTOMER_NAME,
                    CUSTOMER_MIDDLE_NAME = customer.CUSTOMER_MIDDLE_NAME,
                    CUSTOMER_SURNAME = customer.CUSTOMER_SURNAME,
                    CUSTOMER_NUMBER = customer.CUSTOMER_NUMBER,
                    EMAIL = customer.EMAIL,
                    GENDER = customer.GENDER,
                    IDENTIFICATION_ID = customer.IDENTIFICATION_ID,
                    PHONE_NUMBER = customer.PHONE_NUMBER,
                    header = new ResponseHeader
                    {
                        IsSuccess = true,
                        ResponseCode = CommonDefinitions.SUCCESS,
                        ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                    }
                };

                return response;
            }
            catch (Exception ex)
            {
                log.Error("An unexpected error occured while getting customer informations");
                throw ex;
            }
        }

        public static ResponseBoats GetBoatInfo(long boatId)
        {
            try
            {
                Boats boat = boatsService.SelectByBoatId(boatId);
                GetCaptainInfo(boat.CAPTAIN_ID);

                ResponseBoats response = new ResponseBoats
                {
                    BOAT_ID = boat.BOAT_ID,
                    BOAT_INFO = boat.BOAT_INFO,
                    BOAT_NAME = boat.BOAT_NAME,
                    FLAG = boat.FLAG,
                    ROTA_INFO = boat.ROTA_INFO,
                    CAPTAIN_ID = boat.CAPTAIN_ID,
                    QUANTITY = boat.QUANTITY,
                    REGION_ID = boat.REGION_ID,
                    REGION_NAME = GetRegionName(boat.REGION_ID),
                    header = new ResponseHeader
                    {
                        IsSuccess = true,
                        ResponseCode = CommonDefinitions.SUCCESS,
                        ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                    }
                };

                return response;
            }
            catch (Exception ex)
            {
                log.Error("An unexpected error occured while getting customer informations");
                throw ex;
            }
        }

        public static ResponseCaptain GetCaptainInfo(long captainID)
        {
            try
            {
                Captains captain = captainsService.SelectByBoatId(captainID);

                ResponseCaptain response = new ResponseCaptain
                {
                    BOAT_ID = captain.BOAT_ID,
                    CAPTAIN_INFO = captain.CAPTAIN_INFO,
                    CAPTAIN_NAME = captain.CAPTAIN_NAME,
                    CAPTAIN_MIDDLE_NAME = captain.CAPTAIN_MIDDLE_NAME,
                    CAPTAIN_SURNAME = captain.CAPTAIN_SURNAME,
                    EMAIL = captain.EMAIL,
                    IDENTIFICATION_ID = captain.IDENTIFICATION_ID,
                    PHONE_NUMBER = captain.PHONE_NUMBER,
                    CAPTAIN_ID = captain.CAPTAIN_ID,
                    header = new ResponseHeader
                    {
                        IsSuccess = true,
                        ResponseCode = CommonDefinitions.SUCCESS,
                        ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                    }
                };

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetRegionName(int regionID)
        {
            try
            {
                Region region = regionService.SelectByRegionId(regionID);
                ResponseRegion response = new ResponseRegion
                {
                    REGION_ID = region.REGION_ID,
                    REGION_NAME = region.REGION_NAME,
                    header = new ResponseHeader
                    {
                        IsSuccess = true,
                        ResponseCode = CommonDefinitions.SUCCESS,
                        ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                    }
                };

                return response.REGION_NAME;

            }
            catch (Exception ex)
            {
                throw new Exception(CommonDefinitions.REQUEST_ID_NOT_FOUND);
            }
        }

        public static void GetTransactionByCustomerNumber(long customerNumber, ref List<PaymentTransaction> transactions)
        {
            try
            {
                transactions = paymentTransactionService.SelectByCustomerNumber(customerNumber);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void AddTransaction(RequestPayment request, ref bool checkValue)
        {
            try
            {
                long checkguid = 0;
                ResponseBoats responseBoat = GetBoatInfo(request.BOAT_ID);
                ResponsePersonalInformation responseCustomer = GetCustomer(request.CUSTOMER_NUMBER);
                PaymentTransaction transaction = new PaymentTransaction
                {
                    BOAT_ID = request.BOAT_ID,
                    CALLBACK_URL = request.CALLBACK_URL,
                    CARD_HOLDER_NAME = request.CARD_HOLDER_NAME,
                    CARD_REF_NUMBER = request.CARD_REF_NUMBER,
                    CONVERSATION_ID = request.CONVERSATION_ID,
                    CURRENCY = request.CURRENCY,
                    CUSTOMER_NUMBER = request.CUSTOMER_NUMBER,
                    IP = request.IP,
                    PAID_PRICE = request.PAID_PRICE,
                    PRICE = request.PRICE,
                    PAYMENT_CHANNEL = request.PAYMENT_CHANNEL,
                    PAYMENT_ID = request.PAYMENT_ID,
                    TOUR_TYPE = responseBoat.TOUR_TYPE,
                    PAYMENT_TYPE = request.Header.OperationTypes.ToString(),
                    UPDATE_USER = responseCustomer.UPDATE_USER,
                    INSERT_USER = responseCustomer.INSERT_USER

                };
                checkguid = paymentTransactionService.Insert(transaction);
                if (checkguid == 0)
                    checkValue = false;
                else
                    checkValue = true;

                //Reservation date  && id will be update
                Reservation reservation = new Reservation
                {
                    UPDATE_USER = responseCustomer.UPDATE_USER,
                    INSERT_USER = responseCustomer.INSERT_USER,
                    CUSTOMER_NUMBER = request.CUSTOMER_NUMBER,
                    BOAT_ID = request.BOAT_ID,
                    PAYMENT_ID = request.PAYMENT_ID,
                    PAYMENT_TYPE = request.Header.OperationTypes.ToString(),
                    PRICE = request.PRICE,
                    TOUR_TYPE = request.TOUR_TYPE,
                    RESERVATION_DATE = request.RESERVATION_DATE,
                    RESERVATION_END_DATE = request.RESERVATION_END_DATE,
                    RESERVATION_ID = request.RESERVATION_ID,
                    CONFIRM = 0

                };
                //Modify Reservation Informations
                checkValue = reservationService.Update(reservation);
            }
            catch (Exception ex)
            {
                log.Error("AddTransaction has an ERROR: [ERROR : " + ex.Message + "]");
                throw new Exception(ex.Message);
            }
        }

        public static long AddReservation(RequestReservation request, ref bool checkValue)
        {
            try
            {
                long checkguid = 0;

                Reservation reservation = new Reservation
                {
                    UPDATE_USER = request.UPDATE_USER,
                    INSERT_USER = request.INSERT_USER,
                    CUSTOMER_NUMBER = request.CUSTOMER_NUMBER,
                    BOAT_ID = request.BOAT_ID,
                    PAYMENT_ID = request.PAYMENT_ID,
                    PAYMENT_TYPE = request.Header.OperationTypes.ToString(),
                    PRICE = request.PRICE,
                    TOUR_TYPE = request.TOUR_TYPE,
                    RESERVATION_DATE = request.RESERVATION_DATE,
                    RESERVATION_END_DATE = request.RESERVATION_END_DATE,
                    CAPACITY = request.CAPACITY,
                    CONFIRM = request.CONFIRM
                };
                //Add Reservation Informations
                checkguid = reservationService.Insert(reservation);
                if (checkguid == 0)
                    checkValue = false;
                else
                    checkValue = true;

                #region Boats Capacity Calculation
                RequestBoatCapacity requestBoatsCapacity = new RequestBoatCapacity
                {
                    UPDATE_USER = request.UPDATE_USER,
                    INSERT_USER = request.INSERT_USER,
                    BOAT_ID = request.BOAT_ID,
                    RESERVATION_DATE = request.RESERVATION_DATE,
                    RESERVATION_END_DATE = request.RESERVATION_END_DATE,
                    CAPACITY = request.CAPACITY,
                    RESERVATION_ID = checkguid,
                    Header = new Header
                    {
                        OperationTypes = (int)OperationType.OperationTypes.ADD,
                        ApiKey = request.Header.ApiKey,
                        Device = request.Header.Device,
                        RequestId = request.Header.RequestId,
                        TokenId = request.Header.TokenId
                    }
                };
                BoatsCapacityService boatsCapacityService = new BoatsCapacityService();
                BoatCapacityOperation op = new BoatCapacityOperation(requestBoatsCapacity, boatsCapacityService);
                op.Execute();
                if (op.response.header.IsSuccess)
                    checkValue = true;
                else
                    checkValue = false;
                #endregion

                return checkguid;
            }
            catch (Exception ex)
            {
                log.Error("AddReservation has an ERROR: [ERROR : " + ex.Message + "]");
                throw new Exception(ex.Message);
            }
        }

        public static void ModifyReservation(RequestReservation request, ref bool checkValue)
        {
            try
            {
                checkValue = false;
                Reservation reservation = new Reservation
                {
                    UPDATE_USER = request.UPDATE_USER,
                    INSERT_USER = request.INSERT_USER,
                    CUSTOMER_NUMBER = request.CUSTOMER_NUMBER,
                    BOAT_ID = request.BOAT_ID,
                    PAYMENT_ID = request.PAYMENT_ID,
                    PAYMENT_TYPE = request.Header.OperationTypes.ToString(),
                    PRICE = request.PRICE,
                    TOUR_TYPE = request.TOUR_TYPE,
                    RESERVATION_DATE = request.RESERVATION_DATE,
                    CAPACITY = request.CAPACITY,
                    CONFIRM = request.CONFIRM
                };

                if (request.Header.OperationTypes == (int)OperationType.OperationTypes.UPDATE)
                    //Modify Reservation Informations
                    checkValue = reservationService.Update(reservation);
                else if (request.Header.OperationTypes == (int)OperationType.OperationTypes.DELETE)
                    //Delete Reservation Informations
                    checkValue = reservationService.Delete(reservation);

            }
            catch (Exception ex)
            {
                log.Error("AddReservation has an ERROR: [ERROR : " + ex.Message + "]");
                throw new Exception(ex.Message);
            }
        }

        public static List<ResponseBoats> GetPopularBoats(RequestBoats request)
        {
            BaseResponseMessage baseResponseMessage = new BaseResponseMessage();
            //Validate Reques Header / Constants
            baseResponseMessage = Validate(request);
            if (!baseResponseMessage.header.IsSuccess)
                throw new Exception(baseResponseMessage.header.ResponseMessage);

            Boats boat = null;
            List<ResponseBoats> _favorite = new List<ResponseBoats>();
            List<ResponsePopularBoats> respFavorites = new List<ResponsePopularBoats>();
            _favorite = favoritesServices.SelectPopularBoats();

            return _favorite;
        }

        public static BaseResponseMessage Validate(RequestBoats request)
        {
            #region Validation HeaderRequest
            BaseResponseMessage resp = new BaseResponseMessage();
            resp.header = new ResponseHeader();
            if (request.Header.ApiKey != CommonDefinitions.APIKEY)
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.API_KEY_NOT_MATCH;
            }
            else if (request.Header.Device == 0)
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.DEVICE_INFORMATION_NOT_FOUND;
            }
            else if (String.IsNullOrEmpty(request.Header.RequestId))
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


    }
}
