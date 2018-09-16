using Boat.Business.Framework;
using Boat.Business.Operation.UserOperation.PersonalInformation;
using Boat.Data.Dto;
using Boat.Data.Dto.CustomerModule.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Enums = Boat.Business.Framework.Enums;

namespace boat.Test.UserController
{
    [TestClass]
    public class CustomerRelationServiceTest
    {
        public void CustomerRelation()
        {

            RequestCustomerRelation request = new RequestCustomerRelation()
            {
                CUSTOMER_NAME = "pars",
                CUSTOMER_SURNAME = "basmaz",
                CUSTOMER_NUMBER = 1,
                EMAIL = "parsbasmaz@outlook.com",
                INSERT_USER = "testuser",
                IDENTIFICATION_ID = 19034546316,
                PHONE_NUMBER = "5357977314",
                UPDATE_USER = "testuser2",
                GENDER = Enum.GetName(typeof(Enums.GENDER), 1),
                PASSWORD = "Dd123456",
                Header = new Header
                {
                    Device = (int)DeviceEnum.Device.WEB,
                    OperationTypes = (int)OperationType.OperationTypes.ADD,
                    RequestId = Guid.NewGuid().ToString(),
                    ApiKey = "U6l1hKzpZDrgfTaKxpQm3A/6raDiroAI7ueTgMNwhQs=",
                    TokenId = "IM1guWBb1Ui/6+WA2uSQSqhTASnBL2/h"
                }
            };

            CustomerRelationOperation op = new CustomerRelationOperation(request);
            op.Execute();

            Assert.IsTrue(op.responsedata.header.IsSuccess);
        }
    }
}
