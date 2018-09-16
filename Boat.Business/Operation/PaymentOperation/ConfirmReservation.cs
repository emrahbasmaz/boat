using Boat.Backoffice.DataModel.PaymentModule.Entity;
using Boat.Business.Common;
using Boat.Business.Framework;
using Boat.Data;
using Boat.Data.DataModel.PaymentModule.Service.Interface;
using Boat.Data.Dto;
using Boat.Data.Dto.PaymentModule.Request;
using Boat.Data.Dto.PaymentModule.Response;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Boat.Business.Operation.PaymentOperation
{
    public class ConfirmReservation : OperationBase
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IReservationService reservationService;
        //Registration Token 
        public static string[] registrationIds = { "diks4vp5......", "erPjEb9....." };

        public static AndroidGcmPushNotification gcmPushNotification = null;


        public RequestConfirmReservation request = new RequestConfirmReservation();
        public ResponseConfirmReservation response = new ResponseConfirmReservation();
        public BaseResponseMessage baseResponseMessage = null;

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
            else
            {
                resp.header.IsSuccess = true;
                resp.header.ResponseCode = CommonDefinitions.SUCCESS;
                resp.header.ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE;
            }
            #endregion
            return resp;
        }

        public ConfirmReservation(RequestConfirmReservation request)
        {
            this.request.Header = new Header();
            this.request = request;
        }

        public override async void DoOperation()
        {
            //Validate Reques Header / Constants
            this.baseResponseMessage = ValidateInput();
            if (!this.baseResponseMessage.header.IsSuccess)
                throw new Exception(this.baseResponseMessage.header.ResponseMessage);

            this.response = await ConfirmReservations(this.request);
        }

        public override void RollbackOperation()
        {
            throw new NotImplementedException();
        }


        public async Task<ResponseConfirmReservation> ConfirmReservations(RequestConfirmReservation request)
        {
            Reservation reserv = new Reservation();
            reserv = reservationService.SelectByReservationId(request.RESERVATION_ID);
            reserv.CONFIRM = request.CONFIRM;//1 "Y",0 "N"
            if (reservationService.Update(reserv))
            {
                #region Notification
                string message = String.Format("[0] tarihli reservasyonunuz onaylanmıstır.", reserv.RESERVATION_DATE);
                gcmPushNotification = new AndroidGcmPushNotification("API KEY", registrationIds, message);
                string x = gcmPushNotification.SendGcmNotification();
                #endregion

                this.response = new ResponseConfirmReservation
                {
                    RESERVATION_ID = reserv.RESERVATION_ID,
                    CONFIRM = reserv.CONFIRM,
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
                this.response = new ResponseConfirmReservation
                {
                    RESERVATION_ID = reserv.RESERVATION_ID,
                    CONFIRM = reserv.CONFIRM,
                    header = new ResponseHeader
                    {
                        IsSuccess = false,
                        ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_ERROR,
                        ResponseMessage = CommonDefinitions.ERROR_MESSAGE
                    }
                };

            }

            return this.response;
        }
    }
}
