using Boat.Business.Common;
using Boat.Business.Framework;
using Boat.Business.Operation.GeneralOperation;
using Boat.Data.DataModel.GeneralModule.Service;
using Boat.Data.Dto;
using Boat.Data.Dto.BoatModule.Request;
using Boat.Data.Dto.BoatModule.Response;
using Boat.Data.Dto.GeneralModule.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace boat.Test.GeneralController
{
    [TestClass]
    public class FavoritesService
    {
        [TestMethod]
        public void FavoritesOperation()
        {
            FavoritesServices service = new FavoritesServices();
            RequestFavorites fav = new RequestFavorites
            {
                INSERT_USER = "testUser2",
                UPDATE_USER = "testUser2",
                BOAT_ID = 4,
                CUSTOMER_NUMBER = 5,
                Header = new Header
                {
                    Device = (int)DeviceEnum.Device.WEB,
                    OperationTypes = (int)OperationType.OperationTypes.ADD,
                    RequestId = Guid.NewGuid().ToString(),
                    ApiKey = "U6l1hKzpZDrgfTaKxpQm3A/6raDiroAI7ueTgMNwhQs=",
                    TokenId = ""

                }

            };

            FavoriteOperation ff = new FavoriteOperation(fav, service);
            ff.Execute();

            Assert.IsNotNull(ff.response);
            Assert.IsTrue(ff.response.header.IsSuccess == true);

        }

        [TestMethod]
        public void GetPopularBoats()
        {
            RequestBoats req = new RequestBoats
            {
                Header = new Header
                {
                    Device = (int)DeviceEnum.Device.WEB,
                    OperationTypes = (int)OperationType.OperationTypes.UPDATE,
                    RequestId = Guid.NewGuid().ToString(),
                    ApiKey = "U6l1hKzpZDrgfTaKxpQm3A/6raDiroAI7ueTgMNwhQs=",
                    TokenId = ""

                }
            };

            List<ResponseBoats> listFavorites = new List<ResponseBoats>();
            listFavorites = CommonServices.GetPopularBoats(req);

            Assert.IsNotNull(listFavorites);
            Assert.IsTrue((listFavorites.Count > 0 ? true : false));
        }
    }
}
