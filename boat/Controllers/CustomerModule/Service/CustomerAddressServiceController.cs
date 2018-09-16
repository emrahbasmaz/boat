using Boat.Data.Dto.CustomerModule.Request;
using Boat.Data.Dto.CustomerModule.Response;
using Boat.Repository.UserRepository;
using Boat.Repository.UserRepository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace boBoatService.Controllers.CustomerModule.Service
{
    public class CustomerAddressServiceController : ControllerBase
    {
        private readonly AuthRepository repository;

        public CustomerAddressServiceController( )
        {
            this.repository = new AuthRepository();
        }

        [HttpPost]
        [Route("api/customeraddressservice")]
        public async Task<ResponseCustomerAddress> CustomerAddressService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();

            RequestCustomerAddress request = new RequestCustomerAddress();
            request = JsonConvert.DeserializeObject<RequestCustomerAddress>(jsonRequest);
            return await this.repository.CustomerAddressService(request);
        }
    }
}