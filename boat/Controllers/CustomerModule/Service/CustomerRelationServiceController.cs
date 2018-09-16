using Boat.Data.Dto.CustomerModule.Request;
using Boat.Data.Dto.CustomerModule.Response;
using Boat.Repository.UserRepository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace boBoatService.Controllers.CustomerModule.Service
{
    public class CustomerRelationServiceController : ControllerBase
    {

        private readonly IAuthRepository repository;

        public CustomerRelationServiceController(IAuthRepository _repository)
        {
            this.repository = _repository;
        }

        [HttpPost]
        [Route("api/customerrelationservice")]
        public async Task<List<ResponseCustomerRelation>> CustomerRelationService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();
            RequestCustomerRelation request = new RequestCustomerRelation();
            request = JsonConvert.DeserializeObject<RequestCustomerRelation>(jsonRequest);
            return await this.repository.CustomerRelationService(request);
        }
    }
}
