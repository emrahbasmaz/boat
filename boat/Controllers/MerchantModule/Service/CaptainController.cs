using Boat.Data.Dto;
using Boat.Data.Dto.BoatModule.Request;
using Boat.Repository.MerchantRepository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace boBoatService.Controllers.MerchantModule.Service
{
    public class CaptainController : ControllerBase
    {
        private readonly IMerchantRepository repository;
        public CaptainController(IMerchantRepository _repository)
        {
            this.repository = _repository;
        }

        [HttpPost]
        [Route("api/captainservice")]
        public async Task<BaseResponseMessage> CaptainService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();

            RequestCaptain request = new RequestCaptain();
            request = JsonConvert.DeserializeObject<RequestCaptain>(jsonRequest);

            return await this.repository.CaptainService(request);
        }
    }
}
