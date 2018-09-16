using Boat.Data.Dto;
using Boat.Data.Dto.PaymentModule.Request;
using Boat.Repository.PaymentRepository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace boBoatService.Controllers.PaymentModule.Service
{
    public class ReservationController : ControllerBase
    {
        private readonly IPaymentRepository repository;
        public ReservationController(IPaymentRepository _repository)
        {
            this.repository = _repository;
        }

        [HttpPost]
        [Route("api/reservationservice")]
        public async Task<BaseResponseMessage> ReservationService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();

            RequestReservation request = new RequestReservation();
            request = JsonConvert.DeserializeObject<RequestReservation>(jsonRequest);

            return await this.repository.ReservationService(request);
        }
    }
}
