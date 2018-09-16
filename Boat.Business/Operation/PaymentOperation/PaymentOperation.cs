using Boat.Business.Common;
using Boat.Business.Framework;
using Boat.Data;
using Boat.Data.DataModel.PaymentModule.Service;
using Boat.Data.DataModel.PaymentModule.Service.Interface;
using Boat.Data.Dto;
using Boat.Data.Dto.BoatModule.Response;
using Boat.Data.Dto.CustomerModule.Response;
using Boat.Data.Dto.PaymentModule.Request;
using Boat.Data.Dto.PaymentModule.Response;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Enums = Boat.Business.Framework.Enums;

namespace Boat.Business.Operation.PaymentOperation
{
    public class PaymentOperation : OperationBase
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IPaymentTransactionService paymentTransactionService;
        public RequestPayment request = new RequestPayment();
        public ResponsePayment response = new ResponsePayment();
        public BaseResponseMessage baseResponseMessage = null;

        public PaymentOperation(RequestPayment request, PaymentTransactionService service)
        {
            this.request.Header = new Header();
            this.request = request;
            this.paymentTransactionService = service;
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

                Options baseHeader = null;
                string errMsg = "";
                string errCode = "";
                bool result = true;

                //Operation
                switch (this.request.Header.OperationTypes)
                {
                    case (int)OperationType.OperationTypes.ADD:
                        #region PAYMENT

                        //Create payments
                        Iyzipay.Request.CreatePaymentRequest paymentRequest = PrepareRequest(ref result, ref errMsg, ref baseHeader, ref errCode);
                        Payment payment = Payment.Create(paymentRequest, baseHeader);

                        RetrievePaymentRequest request = new RetrievePaymentRequest();
                        request.Locale = Locale.TR.ToString();
                        request.ConversationId = payment.ConversationId;
                        request.PaymentId = payment.PaymentId;
                        request.PaymentConversationId = payment.ConversationId;

                        //check payments
                        Payment Checkpayments = Payment.Retrieve(request, baseHeader);
                        if (Checkpayments.Status == Status.FAILURE.ToString())
                            throw new Exception("Odeme basarısız");

                        bool checkvalue = false;
                        //add transaction
                        CommonServices.AddTransaction(this.request, ref checkvalue);
                        //response
                        this.response = new ResponsePayment
                        {
                            CALLBACK_URL = paymentRequest.CallbackUrl,
                            CARD_HOLDER_NAME = paymentRequest.PaymentCard.CardHolderName,
                            CARD_REF_NUMBER = paymentRequest.PaymentCard.CardNumber,
                            CONVERSATION_ID = payment.ConversationId,
                            CURRENCY = payment.Currency,
                            CUSTOMER_NUMBER = Convert.ToInt64(paymentRequest.Buyer.Id),
                            IP = paymentRequest.Buyer.Ip,
                            PAID_PRICE = payment.PaidPrice,
                            PRICE = payment.Price,
                            PAYMENT_CHANNEL = paymentRequest.PaymentChannel,
                            PAYMENT_ID = Checkpayments.PaymentId,
                            header = new ResponseHeader
                            {
                                IsSuccess = checkvalue == false ? false : true,
                                ResponseCode = checkvalue == false ? CommonDefinitions.INTERNAL_TRANSACTION_ERROR : CommonDefinitions.SUCCESS,
                                ResponseMessage = checkvalue == false ? CommonDefinitions.ERROR_MESSAGE : CommonDefinitions.SUCCESS_MESSAGE
                            }
                        };

                        #endregion
                        break;
                    case (int)OperationType.OperationTypes.DELETE:
                        #region REFUND

                        //Create ReFund
                        FillOptionHeader(ref baseHeader);
                        CreateRefundRequest refundRequest = new CreateRefundRequest();
                        refundRequest.ConversationId = this.request.CONVERSATION_ID;
                        refundRequest.Locale = Locale.TR.ToString();
                        refundRequest.PaymentTransactionId = this.request.PAYMENT_ID;
                        refundRequest.Price = this.request.PRICE;
                        refundRequest.Ip = this.request.IP;
                        refundRequest.Currency = this.request.CURRENCY;

                        //check refund
                        Refund refund = Refund.Create(refundRequest, baseHeader);
                        if (refund.Status == Status.FAILURE.ToString())
                            throw new Exception(" Geri ödeme basarısız");

                        //Transaction
                        checkvalue = false;
                        //add transaction
                        CommonServices.AddTransaction(this.request, ref checkvalue);

                        //response
                        this.response = new ResponsePayment
                        {
                            CALLBACK_URL = this.request.CALLBACK_URL,
                            CARD_HOLDER_NAME = this.request.CARD_HOLDER_NAME,
                            CARD_REF_NUMBER = this.request.CARD_REF_NUMBER,
                            CONVERSATION_ID = refundRequest.ConversationId,
                            CURRENCY = refundRequest.Currency,
                            CUSTOMER_NUMBER = this.request.CUSTOMER_NUMBER,
                            IP = refundRequest.Ip,
                            PAID_PRICE = refundRequest.Price,
                            PRICE = refundRequest.Price,
                            PAYMENT_CHANNEL = this.request.PAYMENT_CHANNEL,
                            PAYMENT_ID = refundRequest.PaymentTransactionId,
                            header = new ResponseHeader
                            {
                                IsSuccess = checkvalue == false ? false : true,
                                ResponseCode = checkvalue == false ? CommonDefinitions.INTERNAL_TRANSACTION_ERROR : CommonDefinitions.SUCCESS,
                                ResponseMessage = checkvalue == false ? CommonDefinitions.ERROR_MESSAGE : CommonDefinitions.SUCCESS_MESSAGE
                            }
                        };

                        #endregion
                        break;
                    case (int)OperationType.OperationTypes.UPDATE:
                        #region BKM PAYMENT

                        CreateBkmInitializeRequest requestBKM = PrepareBkmRequest(ref baseHeader);
                        BkmInitialize bkmInitialize = BkmInitialize.Create(requestBKM, baseHeader);

                        RetrieveBkmRequest retrieveBKM = new RetrieveBkmRequest();
                        retrieveBKM.Locale = Locale.TR.ToString();
                        retrieveBKM.ConversationId = "123456789";
                        retrieveBKM.Token = "token";

                        Bkm bkm = Bkm.Retrieve(retrieveBKM, baseHeader);
                        if (bkm.Status == Status.FAILURE.ToString())
                            throw new Exception("Odeme basarısız");

                        //Transaction
                        checkvalue = false;
                        //add transaction
                        CommonServices.AddTransaction(this.request, ref checkvalue);
                        //response
                        this.response = new ResponsePayment
                        {
                            CALLBACK_URL = requestBKM.CallbackUrl,
                            CARD_HOLDER_NAME = "",
                            CARD_REF_NUMBER = "",
                            CONVERSATION_ID = requestBKM.ConversationId,
                            CURRENCY = "",
                            CUSTOMER_NUMBER = Convert.ToInt64(requestBKM.Buyer.Id),
                            IP = requestBKM.Buyer.Ip,
                            PAID_PRICE = requestBKM.Price,
                            PRICE = requestBKM.Price,
                            PAYMENT_CHANNEL = requestBKM.PaymentSource,
                            PAYMENT_ID = requestBKM.BasketId,
                            header = new ResponseHeader
                            {
                                IsSuccess = checkvalue == false ? false : true,
                                ResponseCode = checkvalue == false ? CommonDefinitions.INTERNAL_TRANSACTION_ERROR : CommonDefinitions.SUCCESS,
                                ResponseMessage = checkvalue == false ? CommonDefinitions.ERROR_MESSAGE : CommonDefinitions.SUCCESS_MESSAGE
                            }
                        };
                        #endregion
                        break;
                    case (int)OperationType.OperationTypes.GET:
                        #region 3D PAYMENT
                        //Initialize 3D Payment
                        Iyzipay.Request.CreatePaymentRequest payment3DRequest = Prepare3DRequest(ref baseHeader);
                        ThreedsInitialize threedsInitialize = ThreedsInitialize.Create(payment3DRequest, baseHeader);
                        if (threedsInitialize.Status == Status.FAILURE.ToString())
                            throw new Exception("Odeme basarısız");
                        //Create 3D Payment
                        CreateThreedsPaymentRequest create3Drequestpayment = new CreateThreedsPaymentRequest();
                        create3Drequestpayment.Locale = Locale.TR.ToString();
                        create3Drequestpayment.ConversationId = payment3DRequest.ConversationId;
                        create3Drequestpayment.PaymentId = this.request.PAYMENT_ID;
                        create3Drequestpayment.ConversationData = "conversation data";// ?????

                        ThreedsPayment threedsPayment = ThreedsPayment.Create(create3Drequestpayment, baseHeader);
                        if (threedsPayment.Status == Status.FAILURE.ToString())
                            throw new Exception("Odeme basarısız");

                        //Transaction
                        checkvalue = false;
                        //add transaction
                        CommonServices.AddTransaction(this.request, ref checkvalue);
                        //response
                        this.response = new ResponsePayment
                        {
                            CALLBACK_URL = payment3DRequest.CallbackUrl,
                            CARD_HOLDER_NAME = payment3DRequest.PaymentCard.CardHolderName,
                            CARD_REF_NUMBER = payment3DRequest.PaymentCard.CardNumber,
                            CONVERSATION_ID = payment3DRequest.ConversationId,
                            CURRENCY = payment3DRequest.Currency,
                            CUSTOMER_NUMBER = Convert.ToInt64(payment3DRequest.Buyer.Id),
                            IP = payment3DRequest.Buyer.Ip,
                            PAID_PRICE = payment3DRequest.PaidPrice,
                            PRICE = payment3DRequest.Price,
                            PAYMENT_CHANNEL = payment3DRequest.PaymentChannel,
                            PAYMENT_ID = create3Drequestpayment.PaymentId,
                            header = new ResponseHeader
                            {
                                IsSuccess = checkvalue == false ? false : true,
                                ResponseCode = checkvalue == false ? CommonDefinitions.INTERNAL_TRANSACTION_ERROR : CommonDefinitions.SUCCESS,
                                ResponseMessage = checkvalue == false ? CommonDefinitions.ERROR_MESSAGE : CommonDefinitions.SUCCESS_MESSAGE
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
                log.Error("Payment Operation has an ERROR: [ERROR : " + ex.Message + "]");
                throw new Exception("Ödeme sirasinda hata oluştu.");
            }
        }

        public override void RollbackOperation()
        {
            throw new NotImplementedException();
        }


        private Iyzipay.Request.CreatePaymentRequest PrepareRequest(ref bool result, ref string errMsg, ref Options baseHeader, ref string errCode)
        {
            try
            {

                Iyzipay.Request.CreatePaymentRequest paymentRequest = new Iyzipay.Request.CreatePaymentRequest();
                ResponsePersonalInformation responseCustomer = CommonServices.GetCustomer(this.request.CUSTOMER_NUMBER);
                ResponseCustomerAddress responseCustomerAddress = CommonServices.GetCustomerAddress(this.request.CUSTOMER_NUMBER);
                ResponseBoats responseBoat = CommonServices.GetBoatInfo(this.request.BOAT_ID);

                baseHeader = new Options();
                baseHeader.ApiKey = this.request.ApiKey;// "sandbox-lJr5mWuuVSnwTa5Dt8bE6ohOi6chI463";
                baseHeader.SecretKey = this.request.SecretKey;// "sandbox-yLkfxt1paeOWTZjV7qzn3rwyFPRrC6Cj";
                baseHeader.BaseUrl = this.request.BaseUrl;// "https://sandbox-merchant.iyzipay.com";

                string cvc = "";
                if (String.IsNullOrEmpty(this.request.PaymentCard.CARD_CVV))
                {
                    cvc = "000";
                }
                else
                {
                    cvc = this.request.PaymentCard.CARD_CVV;
                }

                paymentRequest.PaymentCard = new Iyzipay.Model.PaymentCard()
                {
                    CardHolderName = this.request.PaymentCard.CARD_HOLDER_NAME,
                    CardNumber = Common.CipherAlgorithm.Decrypt(this.request.PaymentCard.CARD_REF_NUMBER, CipherAlgorithm.password),
                    ExpireMonth = this.request.PaymentCard.CARD_EXPIRE_MONTH,
                    ExpireYear = this.request.PaymentCard.CARD_EXPIRE_YEAR,
                    Cvc = cvc,
                    RegisterCard = 0//default
                };


                paymentRequest.Buyer = new Iyzipay.Model.Buyer()
                {
                    Id = this.request.CUSTOMER_NUMBER.ToString(),
                    Name = responseCustomer.CUSTOMER_NAME,
                    Surname = responseCustomer.CUSTOMER_SURNAME,
                    GsmNumber = responseCustomer.PHONE_NUMBER,
                    Email = responseCustomer.EMAIL,
                    IdentityNumber = responseCustomer.IDENTIFICATION_ID.ToString(),
                    LastLoginDate = DateTime.Now.ToShortDateString(),
                    RegistrationDate = DateTime.Now.ToShortDateString(),
                    RegistrationAddress = responseCustomerAddress.DESCRIPTION,
                    Ip = this.request.IP,
                    City = responseCustomerAddress.CITY,
                    Country = responseCustomerAddress.COUNTRY,
                    ZipCode = responseCustomerAddress.ZIPCODE
                };

                paymentRequest.ShippingAddress = new Iyzipay.Model.Address()
                {
                    ContactName = responseCustomer.CUSTOMER_NAME + " " + responseCustomer.CUSTOMER_SURNAME,
                    City = responseCustomerAddress.CITY,
                    Country = responseCustomerAddress.COUNTRY,
                    ZipCode = responseCustomerAddress.ZIPCODE,
                    Description = responseCustomerAddress.DESCRIPTION
                };

                paymentRequest.BillingAddress = new Iyzipay.Model.Address()
                {
                    ContactName = responseCustomer.CUSTOMER_NAME + " " + responseCustomer.CUSTOMER_SURNAME,
                    City = responseCustomerAddress.CITY,
                    Country = responseCustomerAddress.COUNTRY,
                    ZipCode = responseCustomerAddress.ZIPCODE,
                    Description = responseCustomerAddress.DESCRIPTION
                };

                List<BasketItem> basketItems = new List<BasketItem>();
                BasketItem firstBasketItem = new BasketItem();
                firstBasketItem.Id = GenerateNumberManager.GenerateBasketId();
                firstBasketItem.Name = responseBoat.BOAT_NAME;
                firstBasketItem.Category1 = responseBoat.TOUR_TYPE;
                firstBasketItem.Category2 = Enum.GetName(typeof(Enums.TourType), responseBoat.TOUR_TYPE);
                firstBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
                firstBasketItem.Price = responseBoat.TOUR_TYPE == "1" ? responseBoat.PRICE : responseBoat.PRIVATE_PRICE;
                basketItems.Add(firstBasketItem);

                paymentRequest.BasketItems = basketItems;

                paymentRequest.Locale = Locale.TR.ToString();
                paymentRequest.ConversationId = this.request.CONVERSATION_ID;
                paymentRequest.Price = this.request.PRICE;
                paymentRequest.PaidPrice = (Convert.ToDecimal(this.request.PAID_PRICE) / 100).ToString().Replace(',', '.');
                paymentRequest.Installment = 0;
                paymentRequest.BasketId = firstBasketItem.Id;//Sepet Id'si.
                paymentRequest.PaymentChannel = this.request.PAYMENT_CHANNEL.ToString();
                paymentRequest.PaymentGroup = PaymentGroup.PRODUCT.ToString();

                switch (this.request.CURRENCY)
                {
                    case "949":
                        paymentRequest.Currency = Iyzipay.Model.Currency.TRY.ToString();
                        break;
                    case "840":
                        paymentRequest.Currency = Iyzipay.Model.Currency.USD.ToString();
                        break;
                    case "978":
                        paymentRequest.Currency = Iyzipay.Model.Currency.EUR.ToString();
                        break;
                    case "826":
                        paymentRequest.Currency = Iyzipay.Model.Currency.GBP.ToString();
                        break;
                    case "364":
                        paymentRequest.Currency = Iyzipay.Model.Currency.IRR.ToString();
                        break;
                    default:
                        paymentRequest.Currency = "";
                        break;
                }

                if (String.IsNullOrEmpty(paymentRequest.Currency))
                {
                    errCode = CommonDefinitions.INTERNAL_SYSTEM_ERROR;
                    errMsg = CommonDefinitions.CURRENCY_CODE_IS_NOT_VALID;
                    result = false;
                    log.Info("iyzico Sale Call.. Error on PrepareRequest. Detail : " + errMsg);
                }
                return paymentRequest;
            }
            catch (Exception ex)
            {
                log.Error("Prepare payment data has occured an ERROR. [Error: " + ex.Message + "]");
                throw ex;
            }

        }

        private CreateBkmInitializeRequest PrepareBkmRequest(ref Options baseHeader)
        {
            try
            {
                ResponsePersonalInformation responseCustomer = CommonServices.GetCustomer(this.request.CUSTOMER_NUMBER);
                ResponseCustomerAddress responseCustomerAddress = CommonServices.GetCustomerAddress(this.request.CUSTOMER_NUMBER);
                ResponseBoats responseBoat = CommonServices.GetBoatInfo(this.request.BOAT_ID);

                baseHeader = new Options();
                baseHeader.ApiKey = this.request.ApiKey;// "sandbox-lJr5mWuuVSnwTa5Dt8bE6ohOi6chI463";
                baseHeader.SecretKey = this.request.SecretKey;// "sandbox-yLkfxt1paeOWTZjV7qzn3rwyFPRrC6Cj";
                baseHeader.BaseUrl = this.request.BaseUrl;// "https://sandbox-merchant.iyzipay.com";

                CreateBkmInitializeRequest paymentRequest = new CreateBkmInitializeRequest();
                paymentRequest.Locale = Locale.TR.ToString();
                paymentRequest.ConversationId = this.request.CONVERSATION_ID;
                paymentRequest.Price = "1";
                paymentRequest.BasketId = GenerateNumberManager.GenerateBasketId();
                paymentRequest.PaymentGroup = PaymentGroup.PRODUCT.ToString();
                paymentRequest.CallbackUrl = "https://www.merchant.com/callback";


                paymentRequest.Buyer = new Iyzipay.Model.Buyer()
                {
                    Id = this.request.CUSTOMER_NUMBER.ToString(),
                    Name = responseCustomer.CUSTOMER_NAME,
                    Surname = responseCustomer.CUSTOMER_SURNAME,
                    GsmNumber = responseCustomer.PHONE_NUMBER,
                    Email = responseCustomer.EMAIL,
                    IdentityNumber = responseCustomer.IDENTIFICATION_ID.ToString(),
                    LastLoginDate = DateTime.Now.ToShortDateString(),
                    RegistrationDate = DateTime.Now.ToShortDateString(),
                    RegistrationAddress = responseCustomerAddress.DESCRIPTION,
                    Ip = this.request.IP,
                    City = responseCustomerAddress.CITY,
                    Country = responseCustomerAddress.COUNTRY,
                    ZipCode = responseCustomerAddress.ZIPCODE
                };

                paymentRequest.ShippingAddress = new Iyzipay.Model.Address()
                {
                    ContactName = responseCustomer.CUSTOMER_NAME + " " + responseCustomer.CUSTOMER_SURNAME,
                    City = responseCustomerAddress.CITY,
                    Country = responseCustomerAddress.COUNTRY,
                    ZipCode = responseCustomerAddress.ZIPCODE,
                    Description = responseCustomerAddress.DESCRIPTION
                };

                paymentRequest.BillingAddress = new Iyzipay.Model.Address()
                {
                    ContactName = responseCustomer.CUSTOMER_NAME + " " + responseCustomer.CUSTOMER_SURNAME,
                    City = responseCustomerAddress.CITY,
                    Country = responseCustomerAddress.COUNTRY,
                    ZipCode = responseCustomerAddress.ZIPCODE,
                    Description = responseCustomerAddress.DESCRIPTION
                };

                List<BasketItem> basketItems = new List<BasketItem>();
                BasketItem firstBasketItem = new BasketItem();
                firstBasketItem.Id = paymentRequest.BasketId;//Sepet ID si geliştirilecek
                firstBasketItem.Name = responseBoat.BOAT_NAME;
                firstBasketItem.Category1 = responseBoat.TOUR_TYPE;
                firstBasketItem.Category2 = Enum.GetName(typeof(Enums.TourType), responseBoat.TOUR_TYPE);
                firstBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
                firstBasketItem.Price = responseBoat.TOUR_TYPE == "1" ? responseBoat.PRICE : responseBoat.PRIVATE_PRICE;
                basketItems.Add(firstBasketItem);

                paymentRequest.BasketItems = basketItems;


                return paymentRequest;

            }
            catch (Exception)
            {

                throw;
            }


        }

        private CreatePaymentRequest Prepare3DRequest(ref Options baseHeader)
        {
            try
            {
                ResponsePersonalInformation responseCustomer = CommonServices.GetCustomer(this.request.CUSTOMER_NUMBER);
                ResponseCustomerAddress responseCustomerAddress = CommonServices.GetCustomerAddress(this.request.CUSTOMER_NUMBER);
                ResponseBoats responseBoat = CommonServices.GetBoatInfo(this.request.BOAT_ID);

                baseHeader = new Options();
                baseHeader.ApiKey = this.request.ApiKey;// "sandbox-lJr5mWuuVSnwTa5Dt8bE6ohOi6chI463";
                baseHeader.SecretKey = this.request.SecretKey;// "sandbox-yLkfxt1paeOWTZjV7qzn3rwyFPRrC6Cj";
                baseHeader.BaseUrl = this.request.BaseUrl;// "https://sandbox-merchant.iyzipay.com";

                CreatePaymentRequest paymentRequest = new CreatePaymentRequest();
                paymentRequest.Locale = Locale.TR.ToString();
                paymentRequest.ConversationId = this.request.CONVERSATION_ID;
                paymentRequest.Price = this.request.PRICE;
                paymentRequest.PaidPrice = this.request.PAID_PRICE;
                paymentRequest.Currency = Iyzipay.Model.Currency.TRY.ToString();
                paymentRequest.Installment = 1;
                paymentRequest.BasketId = GenerateNumberManager.GenerateBasketId();
                paymentRequest.PaymentChannel = PaymentChannel.WEB.ToString();
                paymentRequest.PaymentGroup = PaymentGroup.PRODUCT.ToString();
                paymentRequest.CallbackUrl = "https://www.merchant.com/callback";

                paymentRequest.Buyer = new Iyzipay.Model.Buyer()
                {
                    Id = this.request.CUSTOMER_NUMBER.ToString(),
                    Name = responseCustomer.CUSTOMER_NAME,
                    Surname = responseCustomer.CUSTOMER_SURNAME,
                    GsmNumber = responseCustomer.PHONE_NUMBER,
                    Email = responseCustomer.EMAIL,
                    IdentityNumber = responseCustomer.IDENTIFICATION_ID.ToString(),
                    LastLoginDate = DateTime.Now.ToShortDateString(),
                    RegistrationDate = DateTime.Now.ToShortDateString(),
                    RegistrationAddress = responseCustomerAddress.DESCRIPTION,
                    Ip = this.request.IP,
                    City = responseCustomerAddress.CITY,
                    Country = responseCustomerAddress.COUNTRY,
                    ZipCode = responseCustomerAddress.ZIPCODE
                };

                paymentRequest.ShippingAddress = new Iyzipay.Model.Address()
                {
                    ContactName = responseCustomer.CUSTOMER_NAME + " " + responseCustomer.CUSTOMER_SURNAME,
                    City = responseCustomerAddress.CITY,
                    Country = responseCustomerAddress.COUNTRY,
                    ZipCode = responseCustomerAddress.ZIPCODE,
                    Description = responseCustomerAddress.DESCRIPTION
                };

                paymentRequest.BillingAddress = new Iyzipay.Model.Address()
                {
                    ContactName = responseCustomer.CUSTOMER_NAME + " " + responseCustomer.CUSTOMER_SURNAME,
                    City = responseCustomerAddress.CITY,
                    Country = responseCustomerAddress.COUNTRY,
                    ZipCode = responseCustomerAddress.ZIPCODE,
                    Description = responseCustomerAddress.DESCRIPTION
                };

                List<BasketItem> basketItems = new List<BasketItem>();
                BasketItem firstBasketItem = new BasketItem();
                firstBasketItem.Id = paymentRequest.BasketId;//Sepet ID si geliştirilecek
                firstBasketItem.Name = responseBoat.BOAT_NAME;
                firstBasketItem.Category1 = responseBoat.TOUR_TYPE;
                firstBasketItem.Category2 = Enum.GetName(typeof(Enums.TourType), responseBoat.TOUR_TYPE);
                firstBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
                firstBasketItem.Price = responseBoat.TOUR_TYPE == "1" ? responseBoat.PRICE : responseBoat.PRIVATE_PRICE;
                basketItems.Add(firstBasketItem);

                paymentRequest.BasketItems = basketItems;


                return paymentRequest;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void FillOptionHeader(ref Options baseHeader)
        {
            try
            {
                baseHeader = new Options();
                baseHeader.ApiKey = this.request.ApiKey;// "sandbox-lJr5mWuuVSnwTa5Dt8bE6ohOi6chI463";
                baseHeader.SecretKey = this.request.SecretKey;// "sandbox-yLkfxt1paeOWTZjV7qzn3rwyFPRrC6Cj";
                baseHeader.BaseUrl = this.request.BaseUrl;// "https://sandbox-merchant.iyzipay.com";
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}
