using Boat.Backoffice.Common;
using Boat.Backoffice.DataModel.PaymentModule.Entity;
using Boat.Backoffice.DataModel.PaymentModule.RequestMessages;
using Boat.Backoffice.DataModel.PaymentModule.ResponseMessages;
using Boat.Backoffice.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Boat.Backoffice.Controller.PaymentController
{
    public class ReservationOperation : OperationBase
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RequestReservation request = new RequestReservation();
        public ResponseReservation response = new ResponseReservation();
        public List<ResponseReservation> listResponse = new List<ResponseReservation>();
        List<Reservation> listReservationResponse = new List<Reservation>();
        public BaseResponseMessage baseResponseMessage = null;
        private bool checkValue = false;

        public ReservationOperation(RequestReservation request)
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
            //Validate Reques Header / Constants
            this.baseResponseMessage = ValidateInput();
            if (!this.baseResponseMessage.header.IsSuccess)
                throw new Exception(this.baseResponseMessage.header.ResponseMessage);


            //Operation
            switch (this.request.Header.OperationTypes)
            {
                case (int)OperationType.OperationTypes.ADD:
                    #region Reserve
                    this.request.RESERVATION_ID = CommonServices.AddReservation(this.request, ref checkValue);

                    response = new ResponseReservation
                    {
                        BOAT_ID = this.request.BOAT_ID,
                        CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                        PRICE = this.request.PRICE,
                        PAYMENT_ID = this.request.PAYMENT_ID,
                        PAYMENT_TYPE = this.request.PAYMENT_TYPE,
                        RESERVATION_DATE = this.request.RESERVATION_DATE,
                        RESERVATION_END_DATE = this.request.RESERVATION_END_DATE,
                        TOUR_TYPE = this.request.TOUR_TYPE,
                        RESERVATION_ID = this.request.RESERVATION_ID,
                        CAPACITY = this.request.CAPACITY,
                        CONFIRM = this.request.CONFIRM,
                        header = new ResponseHeader
                        {
                            IsSuccess = checkValue == false ? false : true,
                            ResponseCode = checkValue == false ? CommonDefinitions.INTERNAL_TRANSACTION_ERROR : CommonDefinitions.SUCCESS,
                            ResponseMessage = checkValue == false ? CommonDefinitions.ERROR_MESSAGE : CommonDefinitions.SUCCESS_MESSAGE
                        }
                    };
                    #endregion
                    break;
                case (int)OperationType.OperationTypes.GET:
                    if (this.request.BOAT_ID != 0)
                    {
                        //Get by BoatId
                        listReservationResponse = Reservation.SelectByBoatId(this.request.BOAT_ID);
                    }
                    else if (this.request.CUSTOMER_NUMBER != 0)
                    {
                        //Get by CustomerNumber
                        listReservationResponse = Reservation.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);

                    }
                    else if (!String.IsNullOrEmpty(this.request.PAYMENT_ID))
                    {
                        //Get by PaymentId
                        listReservationResponse = Reservation.SelectByCustomerNumber(this.request.CUSTOMER_NUMBER);
                    }
                    break;
                case (int)OperationType.OperationTypes.UPDATE:
                    #region Modify Reserve
                    CommonServices.ModifyReservation(this.request, ref checkValue);

                    response = new ResponseReservation
                    {
                        BOAT_ID = this.request.BOAT_ID,
                        CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                        PRICE = this.request.PRICE,
                        PAYMENT_ID = this.request.PAYMENT_ID,
                        PAYMENT_TYPE = this.request.PAYMENT_TYPE,
                        RESERVATION_DATE = this.request.RESERVATION_DATE,
                        RESERVATION_END_DATE = this.request.RESERVATION_END_DATE,
                        TOUR_TYPE = this.request.TOUR_TYPE,
                        RESERVATION_ID = this.request.RESERVATION_ID,
                        CAPACITY = this.request.CAPACITY,
                        CONFIRM = this.request.CONFIRM,
                        header = new ResponseHeader
                        {
                            IsSuccess = checkValue == false ? false : true,
                            ResponseCode = checkValue == false ? CommonDefinitions.INTERNAL_TRANSACTION_ERROR : CommonDefinitions.SUCCESS,
                            ResponseMessage = checkValue == false ? CommonDefinitions.ERROR_MESSAGE : CommonDefinitions.SUCCESS_MESSAGE
                        }
                    };
                    #endregion
                    break;
                case (int)OperationType.OperationTypes.DELETE:
                    #region Modify Cancel of Reservation
                    CommonServices.ModifyReservation(this.request, ref checkValue);

                    response = new ResponseReservation
                    {
                        BOAT_ID = this.request.BOAT_ID,
                        CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                        PRICE = this.request.PRICE,
                        PAYMENT_ID = this.request.PAYMENT_ID,
                        PAYMENT_TYPE = this.request.PAYMENT_TYPE,
                        RESERVATION_DATE = this.request.RESERVATION_DATE,
                        RESERVATION_END_DATE = this.request.RESERVATION_END_DATE,
                        TOUR_TYPE = this.request.TOUR_TYPE,
                        RESERVATION_ID = this.request.RESERVATION_ID,
                        CAPACITY = this.request.CAPACITY,
                        CONFIRM = this.request.CONFIRM,
                        header = new ResponseHeader
                        {
                            IsSuccess = checkValue == false ? false : true,
                            ResponseCode = checkValue == false ? CommonDefinitions.INTERNAL_TRANSACTION_ERROR : CommonDefinitions.SUCCESS,
                            ResponseMessage = checkValue == false ? CommonDefinitions.ERROR_MESSAGE : CommonDefinitions.SUCCESS_MESSAGE
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
