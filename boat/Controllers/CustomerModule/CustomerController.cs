using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using boBoatService.Models.CustomerModule;
using Boat.Backoffice.Controller.UserController;
using Boat.Backoffice.Controller.UserController.PersonalInformation;
using Boat.Backoffice.DataModel.CustomerModule.RequestMessages;
using Boat.Backoffice.DataModel.CustomerModule.ResponseMessages;
using Boat.Backoffice.Controller.UserController.Login;
using Boat.Backoffice.Framework;

namespace boBoatService.Controllers.CustomerModule
{
    public class CustomerController
    {
        public bool CustomerOperation(RequestPersonalInformation request, out ResponsePersonalInformation response)
        {

            PersonalInformationOperation op = new PersonalInformationOperation(request);
            op.Execute();
            response = op.response;
            if (!op.response.header.IsSuccess)
                return false;
            return true;
        }

        public bool CustomerRelationOperation(RequestCustomerRelation request, out List<ResponseCustomerRelation> response)
        {
            CustomerRelationOperation op = new CustomerRelationOperation(request);
            op.Execute();
            response = op.response;
            if (op.response == null && op.response.Count < 1)
                return false;

            return true;
        }

        public bool LoginOperation(RequestPersonalInformation request, out BaseResponseMessage response)
        {
            LoginOperation op = new LoginOperation(request);
            op.Execute();
            response = op.baseResponseMessage;

            if (!response.header.IsSuccess)
                return false;

            return true;
        }

        public bool CustomerAddressOperation(RequestCustomerAddress request, out ResponseCustomerAddress response)
        {
            CustomerAddressOperation op = new CustomerAddressOperation(request);
            op.Execute();
            response = op.response;
            if (!op.response.header.IsSuccess)
                return false;
            return true;
        }
    }
}
