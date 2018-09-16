using Boat.Data.Dto;
using Boat.Data.Dto.BoatModule.Request;
using Boat.Data.Dto.BoatModule.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boat.Repository.MerchantRepository.Interface
{
    public interface IMerchantRepository
    {
        Task<BaseResponseMessage> BoatService(RequestBoats request);
        Task<BaseResponseMessage> BoatPhotoService(RequestBoatPhoto request);
        Task<BaseResponseMessage> CaptainService(RequestCaptain request);
        Task<List<ResponseBoats>> GetPopularBoatService(RequestBoats request);

    }
}
