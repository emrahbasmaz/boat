using Boat.Data.Dto;
using Boat.Data.Dto.BoatModule.Request;
using Boat.Repository.MerchantRepository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace boBoatService.Controllers.MerchantModule.Service
{
    public class BoatController : ControllerBase
    {
        private readonly IMerchantRepository repository;
        public BoatController(IMerchantRepository _repository)
        {
            this.repository = _repository;
        }
        [HttpPost]
        [Route("api/boatservice")]
        public async Task<BaseResponseMessage> BoatService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();

            RequestBoats request = new RequestBoats();
            request = JsonConvert.DeserializeObject<RequestBoats>(jsonRequest);
            return await this.repository.BoatService(request);
        }
    }
}
