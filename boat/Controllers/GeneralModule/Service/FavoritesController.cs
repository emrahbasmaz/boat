using Boat.Data.Dto;
using Boat.Data.Dto.GeneralModule.Request;
using Boat.Repository.GeneralRepostiroy.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace boBoatService.Controllers.GeneralModule.Service
{
    public class FavoritesController : ControllerBase
    {
        private readonly IGeneralRepository repository;
        public FavoritesController(IGeneralRepository _repository)
        {
            this.repository = _repository;
        }

        [HttpPost]
        [Route("api/favoriteservice")]
        public async Task<BaseResponseMessage> FavoriteService([FromBody] JObject json)
        {
            string jsonRequest = json.ToString();
            RequestFavorites request = new RequestFavorites();
            request = JsonConvert.DeserializeObject<RequestFavorites>(jsonRequest);

            return await repository.FavoriteService(request);
        }
    }
}
