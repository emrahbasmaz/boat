using Boat.Data.DataModel.PaymentModule.Service;
using Boat.Data.Dto;
using Boat.Data.Dto.PaymentModule.Request;
using System.Threading.Tasks;

namespace Boat.Repository.PaymentRepository.Interface
{
    public interface IPaymentRepository
    {
        Task<BaseResponseMessage> BoatCapacityService(RequestBoatCapacity request);
        Task<BaseResponseMessage> ConfirmReservationService(RequestConfirmReservation request);
        Task<BaseResponseMessage> PaymentService(RequestPayment request);
        Task<BaseResponseMessage> ReservationService(RequestReservation request);
    }
}
