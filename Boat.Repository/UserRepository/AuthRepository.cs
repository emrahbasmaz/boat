using Boat.Business.Operation.UserOperation.Login;
using Boat.Business.Operation.UserOperation.PersonalInformation;
using Boat.Data.DataModel.CustomerModule.Service;
using Boat.Data.DataModel.CustomerModule.Service.Interface;
using Boat.Data.Dto;
using Boat.Data.Dto.CustomerModule.Request;
using Boat.Data.Dto.CustomerModule.Response;
using Boat.Repository.UserRepository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Boat.Repository.UserRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly CustomerService service;
        private readonly CustomerAddressService addressService;
        //private readonly CustomerRelationService relationService;
        public AuthRepository()
        {
            service = new CustomerService();
            addressService = new CustomerAddressService();
            //relationService = new CustomerRelationService();
        }

        public async Task<ResponseCustomerAddress> CustomerAddressService(RequestCustomerAddress request)
        {
            CustomerAddressOperation op = new CustomerAddressOperation(request, addressService);
            op.Execute();
            return op.response;
        }

        public async Task<List<ResponseCustomerRelation>> CustomerRelationService(RequestCustomerRelation request)
        {
            CustomerRelationOperation op = new CustomerRelationOperation(request);
            op.Execute();
            return op.response;
        }

        public async Task<BaseResponseMessage> Login(RequestPersonalInformation request)
        {
            LoginOperation op = new LoginOperation(request, service);
            op.Execute();

            return op.baseResponseMessage;
        }

        public async Task<ResponsePersonalInformation> Register(RequestPersonalInformation request)
        {
            PersonalInformationOperation op = new PersonalInformationOperation(request, service);
            op.Execute();

            return op.response;
        }

        public Task<bool> UserExists(string username)
        {
            throw new NotImplementedException();
        }
    }
}
