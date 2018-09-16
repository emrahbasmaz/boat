using Boat.Data.Dto;
using Boat.Data.Dto.GeneralModule.Request;
using System.Threading.Tasks;

namespace Boat.Repository.GeneralRepostiroy.Interface
{
    public interface IGeneralRepository
    {
        Task<BaseResponseMessage> ComplaintsService(RequestComplaints request);

        Task<BaseResponseMessage> FavoriteService(RequestFavorites request);

        Task<BaseResponseMessage> RegionService(RequestRegion request);

    }
}
