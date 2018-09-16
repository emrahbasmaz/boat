using Boat.Data.Dto;
using Boat.Data.Dto.BoatModule.Request;
using Boat.Repository.MerchantRepository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace boBoatService.Controllers.MerchantModule.Service
{
    public class BoatPhotoController : ControllerBase
    {
        private readonly IMerchantRepository repository;
        public BoatPhotoController(IMerchantRepository _repository)
        {
            this.repository = _repository;
        }

        [HttpPost]
        [Route("api/boatphotoservice")]
        public async Task<BaseResponseMessage> BoatPhotoService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();

            RequestBoatPhoto request = new RequestBoatPhoto();
            request = JsonConvert.DeserializeObject<RequestBoatPhoto>(jsonRequest);

            return await this.repository.BoatPhotoService(request);
        }
    }
}
