
using Boat.Business.Operation.GeneralOperation;
using Boat.Data.DataModel.GeneralModule.Service;
using Boat.Data.Dto;
using Boat.Data.Dto.GeneralModule.Request;
using Boat.Repository.GeneralRepostiroy.Interface;
using System.Threading.Tasks;

namespace Boat.Repository.GeneralRepostiroy
{
    public class GeneralRepository : IGeneralRepository
    {
        private readonly ComplaintsService complaintService;
        private readonly FavoritesServices favoritesServices;
        private readonly RegionService regionService;
        public GeneralRepository()
        {
            this.regionService = new RegionService();
            this.favoritesServices = new FavoritesServices();
            this.complaintService = new ComplaintsService();
        }
        public async Task<BaseResponseMessage> ComplaintsService(RequestComplaints request)
        {
            ComplaintOperation op = new ComplaintOperation(request,this.complaintService);
            op.Execute();
            return op.baseResponseMessage;
        }

        public async Task<BaseResponseMessage> FavoriteService(RequestFavorites request)
        {
            FavoriteOperation op = new FavoriteOperation(request,this.favoritesServices);
            op.Execute();
            return op.baseResponseMessage;

        }

        public async Task<BaseResponseMessage> RegionService(RequestRegion request)
        {
            RegionOperation op = new RegionOperation(request,this.regionService);
            op.Execute();
            return op.baseResponseMessage;
        }
    }
}
