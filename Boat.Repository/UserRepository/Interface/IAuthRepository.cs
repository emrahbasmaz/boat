using Boat.Data.Dto;
using Boat.Data.Dto.CustomerModule.Request;
using Boat.Data.Dto.CustomerModule.Response;
using System.Collections.Generic;using System.Threading.Tasks;

namespace Boat.Repository.UserRepository.Interface
{
    public interface IAuthRepository
    {
        Task<ResponsePersonalInformation> Register(RequestPersonalInformation requestPersonal);

        Task<BaseResponseMessage> Login(RequestPersonalInformation requestPersonal);

        Task<List<ResponseCustomerRelation>> CustomerRelationService(RequestCustomerRelation request);

        Task<ResponseCustomerAddress> CustomerAddressService(RequestCustomerAddress request);

        Task<bool> UserExists(string username);
    }
}
