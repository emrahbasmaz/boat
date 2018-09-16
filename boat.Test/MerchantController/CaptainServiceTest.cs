using System;
using Boat.Business.Framework;
using Boat.Business.Operation.MerchantOperation;
using Boat.Data.DataModel.BoatModule.Service;
using Boat.Data.Dto;
using Boat.Data.Dto.BoatModule.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace boat.Test.MerchantController
{
    [TestClass]
    public class CaptainServiceTest
    {
        [TestMethod]
        public void CaptainOperation()
        {
            CaptainsService service = new CaptainsService();
            RequestCaptain captain = new RequestCaptain
            {
                INSERT_USER = "testUser2",
                UPDATE_USER = "testUser2",
                CAPTAIN_NAME = "dexter",
                CAPTAIN_MIDDLE_NAME = null,
                CAPTAIN_SURNAME = "jason",
                BOAT_ID = 3,
                IDENTIFICATION_ID = 19034546316,
                EMAIL = "dester.jason@hotmail.com",
                PHONE_NUMBER = "5336668877",
                CAPTAIN_INFO = "xxxxxxxxxxxxxxxyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyzzzzzzzzzzzzzzzzzzzzz",
                Header = new Header
                {
                    Device = (int)DeviceEnum.Device.WEB,
                    OperationTypes = (int)OperationType.OperationTypes.ADD,
                    RequestId = Guid.NewGuid().ToString(),
                    ApiKey = "U6l1hKzpZDrgfTaKxpQm3A/6raDiroAI7ueTgMNwhQs=",
                    TokenId = ""

                }
            };

            CaptainOperation cc = new CaptainOperation(captain, service);
            cc.Execute();

            Assert.IsNotNull(cc.response);
            Assert.IsTrue(cc.response.header.IsSuccess == true);

        }
    }
}
