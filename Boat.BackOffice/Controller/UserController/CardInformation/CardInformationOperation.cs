using Boat.Backoffice.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
using Boat.Backoffice.DataModel.PaymentModule.Entity;
using Boat.Backoffice.DataModel.PaymentModule.RequestMessages;
using Boat.Backoffice.DataModel.PaymentModule.ResponseMessages;
using Boat.Backoffice.Common;

namespace Boat.Backoffice.Controller.UserController.CardInformation
{
    public class CardInformationOperation : OperationBase
    {

        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public CardMaster cardInfo = null;
        public RequestCard request = null;
        public ResponseCard response = null;
        public BaseResponseMessage baseResponseMessage = null;

        public CardInformationOperation(RequestCard request)
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
            else if (String.IsNullOrEmpty(this.request.CARD_MASKED_NUMBER))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.IDENTIFICATION_ID_NOT_VALID;
            }
            else if (String.IsNullOrEmpty(this.request.CONVERSATION_ID))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_SYSTEM_VALIDATION_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.IDENTIFICATION_ID_NOT_VALID;
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
            else if (String.IsNullOrEmpty(this.request.CARD_CVV))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_CARD_INFO_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.CVV_NOT_VALID;
            }
            else if (String.IsNullOrEmpty(this.request.CARD_HOLDER_NAME))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_CARD_INFO_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.CARD_NAME_NOT_VALID;
            }
            else if (String.IsNullOrEmpty(this.request.CARD_MASKED_NUMBER) && String.IsNullOrEmpty(this.request.CARD_REF_NUMBER))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_CARD_INFO_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.CARD_NUMBER_NOT_VALID;
            }
            else if (String.IsNullOrEmpty(this.request.CARD_EXPIRE_MONTH) && String.IsNullOrEmpty(this.request.CARD_EXPIRE_DATE) && String.IsNullOrEmpty(this.request.CARD_EXPIRE_YEAR))
            {
                resp.header.IsSuccess = false;
                resp.header.ResponseCode = CommonDefinitions.INTERNAL_CARD_INFO_ERROR;
                resp.header.ResponseMessage = CommonDefinitions.CARD_DATE_NOT_VALID;
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
                throw new Exception(this.response.header.ResponseMessage);

            //Operation
            switch (this.request.Header.OperationTypes)
            {
                case (int)OperationType.OperationTypes.ADD:
                    break;
                case (int)OperationType.OperationTypes.GET:
                    break;
                case (int)OperationType.OperationTypes.UPDATE:
                    break;
                case (int)OperationType.OperationTypes.DELETE:
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
