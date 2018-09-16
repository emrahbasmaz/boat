using Boat.Data.Dto;
using Boat.Data.Dto.GeneralModule.Request;
using Boat.Repository.GeneralRepostiroy.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace boBoatService.Controllers.GeneralModule.Service
{
    public class RegionController : ControllerBase
    {
        private readonly IGeneralRepository repository;
        public RegionController(IGeneralRepository _repository)
        {
            this.repository = _repository;
        }

        [HttpPost]
        [Route("api/regionservice")]
        public async Task<BaseResponseMessage> RegionService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();

            RequestRegion request = new RequestRegion();
            request = JsonConvert.DeserializeObject<RequestRegion>(jsonRequest);
            return await repository.RegionService(request);
        }
    }
}
