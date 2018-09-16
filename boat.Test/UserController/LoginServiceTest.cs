using Boat.Business.Framework;
using Boat.Business.Operation.UserOperation.Login;
using Boat.Data.DataModel.CustomerModule.Service;
using Boat.Data.Dto;
using Boat.Data.Dto.CustomerModule.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using Enums = Boat.Business.Framework.Enums;

namespace boat.Test.UserController
{
    [TestClass]
    public class LoginServiceTest
    {
        [TestMethod]
        public void LoginOperation()
        {
            CustomerService service = new CustomerService();
            string pass = "zzzzzzzzzzzz";
            byte[] bb = Encoding.UTF8.GetBytes(pass);

            RequestPersonalInformation request = new RequestPersonalInformation()
            {
                CUSTOMER_NAME = "john",
                CUSTOMER_SURNAME = "cluster",
                CUSTOMER_NUMBER = 1,
                EMAIL = "cluster@outlook.com",
                INSERT_USER = "testuser",
                IDENTIFICATION_ID = 79945571838,
                PHONE_NUMBER = "5357977315",
                UPDATE_USER = "testuser2",
                GENDER = Enum.GetName(typeof(Enums.GENDER), 1),
                PASSWORD_HASH = bb,
                PASSWORD_SALT = bb,
                Header = new Header
                {
                    Device = (int)DeviceEnum.Device.WEB,
                    OperationTypes = (int)OperationType.OperationTypes.ADD,
                    RequestId = Guid.NewGuid().ToString(),
                    ApiKey = "U6l1hKzpZDrgfTaKxpQm3A/6raDiroAI7ueTgMNwhQs="
                }
            };

            LoginOperation op = new LoginOperation(request, service);
            op.Execute();

            Assert.IsTrue(op.baseResponseMessage.header.IsSuccess);
        }
    }
}
