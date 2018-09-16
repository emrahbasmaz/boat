using Boat.Data.Dto;
using Boat.Data.Dto.PaymentModule.Request;
using Boat.Repository.PaymentRepository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace boBoatService.Controllers.PaymentModule.Service
{
    public class ConfirmReservationController : ControllerBase
    {
        private readonly IPaymentRepository repository;
        public ConfirmReservationController(IPaymentRepository _repository)
        {
            this.repository = _repository;
        }

        [HttpPost]
        [Route("api/confirmreservationservice")]
        public async Task<BaseResponseMessage> ConfirmReservationService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();

            RequestConfirmReservation request = new RequestConfirmReservation();
            request = JsonConvert.DeserializeObject<RequestConfirmReservation>(jsonRequest);

            return await this.repository.ConfirmReservationService(request);
        }
    }
}
