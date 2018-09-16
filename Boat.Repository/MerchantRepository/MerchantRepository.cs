using Boat.Business.Common;
using Boat.Business.Operation.MerchantOperation;
using Boat.Data.DataModel.BoatModule.Service;
using Boat.Data.DataModel.GeneralModule.Service;
using Boat.Data.Dto;
using Boat.Data.Dto.BoatModule.Request;
using Boat.Data.Dto.BoatModule.Response;
using Boat.Repository.MerchantRepository.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boat.Repository.MerchantRepository
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly BoatPhotosService boatPhotosService;
        private readonly BoatsService boatsService;
        private readonly CaptainsService captainsService;
        private readonly FavoritesServices favoritesServices;
        public MerchantRepository()
        {
            this.boatPhotosService = new BoatPhotosService();
            this.boatsService = new BoatsService();
            this.captainsService = new CaptainsService();
            this.favoritesServices = new FavoritesServices();
        }

        public async Task<BaseResponseMessage> BoatPhotoService(RequestBoatPhoto request)
        {
            BoatPhotoOperation op = new BoatPhotoOperation(request, this.boatPhotosService);
            op.Execute();

            return op.response;
        }

        public async Task<BaseResponseMessage> BoatService(RequestBoats request)
        {
            BoatOperation op = new BoatOperation(request, this.boatsService);
            op.Execute();

            return op.response;
        }

        public async Task<BaseResponseMessage> CaptainService(RequestCaptain request)
        {
            CaptainOperation op = new CaptainOperation(request, this.captainsService);
            op.Execute();

            return op.response;
        }

        public async Task<List<ResponseBoats>> GetPopularBoatService(RequestBoats request)
        {
            //ToDo: 
            //Kişiye özel popular secimler gelmeli
            return this.favoritesServices.SelectPopularBoats();
        }
    }
}
