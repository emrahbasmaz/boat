using Boat.Data.Dto;
using Boat.Data.Dto.GeneralModule.Request;
using Boat.Repository.GeneralRepostiroy.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace boBoatService.Controllers.GeneralModule.Service
{
    public class ComplaintsController : ControllerBase
    {
        private readonly IGeneralRepository repository;
        public ComplaintsController(IGeneralRepository _repository)
        {
            this.repository = _repository;
        }

        [HttpPost]
        [Route("api/complaintservice")]
        public async Task<BaseResponseMessage> ComplaintsService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();

            RequestComplaints request = new RequestComplaints();
            request = JsonConvert.DeserializeObject<RequestComplaints>(jsonRequest);
            return await this.repository.ComplaintsService(request);
        }
    }
}
