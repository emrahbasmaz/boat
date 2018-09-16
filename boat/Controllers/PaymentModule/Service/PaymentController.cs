using Boat.Data.Dto;
using Boat.Data.Dto.PaymentModule.Request;
using Boat.Repository.PaymentRepository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace boBoatService.Controllers.PaymentModule.Service
{
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository repository;
        public PaymentController(IPaymentRepository _repository)
        {
            this.repository = _repository;
        }

        [HttpPost]
        [Route("api/paymentservice")]
        public async Task<BaseResponseMessage> PaymentService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();

            RequestPayment request = new RequestPayment();
            request = JsonConvert.DeserializeObject<RequestPayment>(jsonRequest);

            return await this.repository.PaymentService(request);
        }
    }
}
