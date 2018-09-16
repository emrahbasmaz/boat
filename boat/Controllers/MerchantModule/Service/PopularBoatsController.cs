using Boat.Data.Dto.BoatModule.Request;
using Boat.Data.Dto.BoatModule.Response;
using Boat.Repository.MerchantRepository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace boBoatService.Controllers.MerchantModule.Service
{
    public class PopularBoatsController : ControllerBase
    {
        private readonly IMerchantRepository repository;
        public PopularBoatsController(IMerchantRepository _repository)
        {
            this.repository = _repository;
        }

        [HttpPost]
        [Route("api/popularboatsservice")]
        public async Task<List<ResponseBoats>> PopularBoatsService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();
            RequestBoats request = new RequestBoats();
            request = JsonConvert.DeserializeObject<RequestBoats>(jsonRequest);

            return await this.repository.GetPopularBoatService(request);
        }
    }
}
