using Boat.Backoffice.Common;
using Boat.Backoffice.DataModel.GeneralModule.Entity;
using Boat.Backoffice.DataModel.GeneralModule.RequestMessages;
using Boat.Backoffice.DataModel.GeneralModule.ResponseMessages;
using Boat.Backoffice.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Boat.Backoffice.Controller.GeneralController
{
    public class RegionOperation : OperationBase
    {
        static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Region region = null;
        public List<Region> listRegion = null;
        public List<ResponseRegion> listResponseRegion = null;
        public RequestRegion request = new RequestRegion();
        public ResponseRegion response = null;

        public BaseResponseMessage baseResponseMessage = null;

        public RegionOperation(RequestRegion request) //: base(OperationCounter.CustomerRelationOperation)
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

            try
            {
                switch (this.request.Header.OperationTypes)
                {
                    case (int)OperationType.OperationTypes.ADD:
                        #region ADD
                        long checkGuid = 0;
                        this.region = new Region
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            REGION_NAME = this.request.REGION_NAME
                        };
                        checkGuid = Region.Insert(this.region);

                        this.response = new ResponseRegion
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            REGION_ID = checkGuid,
                            REGION_NAME = this.request.REGION_NAME,
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
                        if (this.request.REGION_ID != 0)
                        {
                            this.region = Region.SelectByRegionId(this.request.REGION_ID);
                            response = new ResponseRegion
                            {
                                REGION_ID = this.region.REGION_ID,
                                REGION_NAME = this.region.REGION_NAME,
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
                            this.listRegion = Region.SelectAllRegion();
                            if (this.listRegion != null && this.listRegion.Count() > 0)
                            {
                                listResponseRegion = new List<ResponseRegion>();
                                foreach (var item in this.listRegion)
                                {
                                    this.response = new ResponseRegion
                                    {
                                        REGION_ID = item.REGION_ID,
                                        REGION_NAME = item.REGION_NAME,
                                        header = new ResponseHeader
                                        {
                                            IsSuccess = true,
                                            ResponseCode = CommonDefinitions.SUCCESS,
                                            ResponseMessage = CommonDefinitions.SUCCESS_MESSAGE
                                        }
                                    };

                                    listResponseRegion.Add(this.response);
                                }
                            }
                        }
                        #endregion
                        break;
                    case (int)OperationType.OperationTypes.UPDATE:
                        #region UPDATE
                        this.region = new Region
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            REGION_NAME = this.request.REGION_NAME
                        };
                        Region.Update(this.region);
                        response = new ResponseRegion
                        {
                            REGION_ID = this.region.REGION_ID,
                            REGION_NAME = this.region.REGION_NAME,
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
                        this.region = new Region
                        {
                            INSERT_USER = this.request.INSERT_USER,
                            UPDATE_USER = this.request.UPDATE_USER,
                            REGION_NAME = this.request.REGION_NAME
                        };
                        Region.Delete(this.region);
                        response = new ResponseRegion
                        {
                            REGION_ID = 0,
                            REGION_NAME = "",
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
                throw new Exception(ex.Message);
            }
        }

        public override void RollbackOperation()
        {
            TranSeq = Enums.TransactionSequence.ROLLBACK;
            throw new NotImplementedException();
        }


    }
}
