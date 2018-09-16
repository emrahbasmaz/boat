using Boat.Business.Operation.PaymentOperation;
using Boat.Data.DataModel.PaymentModule.Service;
using Boat.Data.Dto;
using Boat.Data.Dto.PaymentModule.Request;
using Boat.Repository.PaymentRepository.Interface;
using System.Threading.Tasks;

namespace Boat.Repository.PaymentRepository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly BoatsCapacityService boatsCapacityService;
        private readonly CardMasterService cardMasterService;
        private readonly PaymentTransactionService paymentTransactionService;
        private readonly ReservationService reservationService;
        public PaymentRepository()
        {
            this.boatsCapacityService = new BoatsCapacityService();
            this.cardMasterService = new CardMasterService();
            this.paymentTransactionService = new PaymentTransactionService();
            this.reservationService = new ReservationService();
        }
        public async Task<BaseResponseMessage> BoatCapacityService(RequestBoatCapacity request)
        {
            BoatCapacityOperation op = new BoatCapacityOperation(request, this.boatsCapacityService);
            op.Execute();

            return op.baseResponseMessage;
        }

        public async Task<BaseResponseMessage> ConfirmReservationService(RequestConfirmReservation request)
        {
            ConfirmReservation op = new ConfirmReservation(request);
            op.Execute();

            return op.baseResponseMessage;
        }

        public async Task<BaseResponseMessage> PaymentService(RequestPayment request)
        {
            PaymentOperation op = new PaymentOperation(request,this.paymentTransactionService);
            op.Execute();

            return op.baseResponseMessage;
        }

        public async Task<BaseResponseMessage> ReservationService(RequestReservation request)
        {
            ReservationOperation op = new ReservationOperation(request, this.reservationService);
            op.Execute();
            return op.baseResponseMessage;
        }
    }
}
