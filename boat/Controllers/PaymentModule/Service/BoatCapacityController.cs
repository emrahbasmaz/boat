using Boat.Data.Dto;
using Boat.Data.Dto.PaymentModule.Request;
using Boat.Repository.PaymentRepository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
namespace boBoatService.Controllers.PaymentModule.Service
{
    public class BoatCapacityController : ControllerBase
    {
        private readonly IPaymentRepository repository;
        public BoatCapacityController(IPaymentRepository _repository)
        {
            this.repository = _repository;
        }

        [HttpPost]
        [Route("api/capacityservice")]
        public async Task<BaseResponseMessage> BoatCapacityService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();

            RequestBoatCapacity request = new RequestBoatCapacity();
            request = JsonConvert.DeserializeObject<RequestBoatCapacity>(jsonRequest);
            return await this.repository.BoatCapacityService(request);
        }
    }
}
