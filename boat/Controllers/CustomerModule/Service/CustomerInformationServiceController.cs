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
    public class CustomerInformationServiceController : ControllerBase
    {
        private readonly AuthRepository repository;

        public CustomerInformationServiceController()
        {
            this.repository = new AuthRepository();
        }

        [HttpPost]
        [Route("api/customerinformationservice")]
        public Task<ResponsePersonalInformation> CustomerInformationServiceAsync([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();
            RequestPersonalInformation request = new RequestPersonalInformation();
            request = JsonConvert.DeserializeObject<RequestPersonalInformation>(jsonRequest);

            return this.repository.Register(request);
        }
    }
}
