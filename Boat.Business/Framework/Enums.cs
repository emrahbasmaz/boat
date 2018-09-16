using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boat.Business.Framework
{
    public class Enums
    {
        [Serializable]
        public enum GENDER
        {
            MALE = 0,//erkek
            FEMALE = 1//kız
        }

        [Serializable]
        public enum TransactionSequence
        {
            DEFAULT = 0,
            VALIDATION = 1,
            DO_OPERATION = 2,
            COMPLETED = 3,
            ROLLBACK = 4
        }

        public enum Region
        {
            KAS = 1,
            KEKOVA = 2,
            ANTALYA = 3,
            ALANYA = 4,
            MARMARIS = 5,
            FETHIYE = 6,
            BODRUM = 7,
            DIDIM = 8,
            KUSADASI = 9,
            CESME = 10,
            AYVALIK = 11
        }

        public enum TourType
        {
            Daily = 1,
            Private = 2
        }

        public enum PaymentType
        {
            PAYMENT = 1,
            THREED_PAYMENT = 2,
            BKM_PAYMENT = 3,
            REFUND = 4
        }
    }
}
