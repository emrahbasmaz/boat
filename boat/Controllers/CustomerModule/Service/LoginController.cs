using Boat.Data.Dto;
using Boat.Data.Dto.CustomerModule.Request;
using Boat.Repository.UserRepository;
using Boat.Repository.UserRepository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace boBoatService.Controllers.CustomerModule.Service
{
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AuthRepository repository;

        public LoginController()
        {
            this.repository = new AuthRepository();
        }

        [HttpPost]
        [Route("api/loginservice")]
        public async Task<BaseResponseMessage> LoginService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();
            RequestPersonalInformation request = new RequestPersonalInformation();
            request = JsonConvert.DeserializeObject<RequestPersonalInformation>(jsonRequest);

            return await this.repository.Login(request);

        }
    }
}
