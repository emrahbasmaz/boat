using Boat.Business.Framework;
using Boat.Business.Operation.MerchantOperation;
using Boat.Data.DataModel.BoatModule.Service;
using Boat.Data.Dto;
using Boat.Data.Dto.BoatModule.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Enums = Boat.Business.Framework.Enums;

namespace boat.Test.MerchantController
{
    [TestClass]
    public class BoatServiceTest
    {
        [TestMethod]
        public void BoatOperation()
        {
            BoatsService service = new BoatsService();
            RequestBoats requestBoat = new RequestBoats
            {
                BOAT_ID = 3,
                BOAT_INFO = "TestBoat3",
                CAPTAIN_ID = 1,
                FLAG = "TR",
                BOAT_NAME = "TestBoat3",
                INSERT_USER = "testUser3",
                QUANTITY = 14,
                ROTA_INFO = "testRota3",
                UPDATE_USER = "TestUpdateUser3",
                PRICE = "50",
                PRIVATE_PRICE = "150",
                REGION_ID = 2,
                TOUR_TYPE = Enum.GetName(typeof(Enums.TourType), 1),

                Header = new Header
                {
                    Device = (int)DeviceEnum.Device.WEB,
                    OperationTypes = (int)OperationType.OperationTypes.UPDATE,
                    RequestId = Guid.NewGuid().ToString(),
                    ApiKey = "U6l1hKzpZDrgfTaKxpQm3A/6raDiroAI7ueTgMNwhQs=",
                    TokenId = ""

                }
            };

            BoatOperation bb = new BoatOperation(requestBoat, service);
            bb.Execute();

            Assert.IsNotNull(bb.response);
            Assert.IsTrue(bb.response.header.IsSuccess == true);

        }
    }
}
