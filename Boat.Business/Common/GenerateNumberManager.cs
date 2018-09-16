using log4net;
using System;
using System.Reflection;

namespace Boat.Business.Common
{
    public class GenerateNumberManager
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //public static readonly string SEQUENCE_SHADOW_BARCODE = "CRD.SHADOW_BARCODE_SEQ";
        //public static readonly string SEQUENCE_BARCODE = "CRD.BARCODE_SEQ";

        public static string CalculateCheckDigitByEan13(string number)
        {
            if (number.Length != 12)
                throw new Exception("InvalidStringLength");

            int checkSumDigit = 0;
            int oddSum = 0;
            int evenSum = 0;
            for (int i = 0; i < 12; i++)
            {
                if (i % 2 == 0)
                {
                    //çift indexlerin toplamı
                    evenSum += Convert.ToInt32(number.Substring(i, 1));
                }
                else if (i % 2 == 1)
                {
                    // tek indexlerin toplamı
                    oddSum += Convert.ToInt32(number.Substring(i, 1));
                }
            }
            oddSum = oddSum * 3;
            checkSumDigit = (10 - ((evenSum + oddSum) % 10)) % 10;

            return checkSumDigit.ToString();
        }

        public static string GeneratePaymentId()
        {
            string barcode = string.Empty;
            try
            {
                barcode += DateTime.Now.Year.ToString(); //yılın son hanesini al
                barcode += DateTime.Now.Month.ToString(); //ayın son hanesini al
                barcode += DateTime.Now.DayOfYear.ToString().PadLeft(2, '0'); //bugünün yıldaki sırasını al
                barcode += GetPaymentSequence().ToString().PadLeft(4, '0'); //barkod sayaç numarası
                barcode += CalculateCheckDigitByEan13(barcode); // checkdigit
                if (barcode.Length != 13)
                    throw new Exception("InvalidParameter");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return barcode;
        }

        public static string GenerateCardRef()
        {
            string barcode = string.Empty;
            try
            {
                barcode += DateTime.Now.Year.ToString(); //yılın son hanesini al
                barcode += DateTime.Now.Month.ToString(); //ayın son hanesini al
                barcode += DateTime.Now.DayOfYear.ToString().PadLeft(2, '0'); //bugünün yıldaki sırasını al
                barcode += GetPaymentSequence().ToString().PadLeft(4, '0'); //barkod sayaç numarası
                barcode += CalculateCheckDigitByEan13(barcode); // checkdigit
                if (barcode.Length != 13)
                    throw new Exception("InvalidParameter");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return barcode;
        }

        public static string GenerateCustomerNumber()
        {
            string barcode = string.Empty;
            try
            {
                string constName = "11";
                barcode += constName;// sabit 2 hane
                barcode += DateTime.Now.Year.ToString(); //yılın son hanesini al
                barcode += GetCustomerSequence().ToString().PadLeft(6, '0'); //barkod sayaç numarası
                barcode += CalculateCheckDigitByEan13(barcode); // checkdigit

                if (barcode.Length != 13)
                    throw new Exception("Invalid Number");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return barcode;
        }

        public static string GenerateBasketId()
        {
            string barcode = "BI";
            try
            {
                barcode += DateTime.Now.Year.ToString();  //yılın son hanesini al
                barcode += DateTime.Now.Month.ToString(); //ayın son hanesini al
                barcode += DateTime.Now.DayOfYear.ToString().PadLeft(2, '0'); //bugünün yıldaki sırasını al
                barcode += GetPaymentSequence().ToString().PadLeft(4, '0'); //barkod sayaç numarası
                //barcode += CalculateCheckDigitByEan13(barcode); // checkdigit

                if (barcode.Length != 13)
                    throw new Exception("InvalidParameter");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return barcode;
        }

        //public static CardNumberEntity GenerateCardNumbers(string cardNumber)
        //{
        //    if (string.IsNullOrEmpty(cardNumber))
        //        throw new Exception("MissingCardNumber");

        //    //CardNumberEntity cardNumberEntity = new CardNumberEntity();
        //    //cardNumberEntity.CardNumber = cardNumber;
        //    //cardNumberEntity.EncryptedCardNumber = CardNumberUtility.EncryptCardNumber(cardNumberEntity.CardNumber);
        //    //cardNumberEntity.MaskedCardNumber = CardNumberUtility.MaskCardNumber(cardNumberEntity.CardNumber);
        //    //cardNumberEntity.HashCardNumber = CardNumberUtility.GenerateHashCardNumber(cardNumberEntity.CardNumber);
        //    //cardNumberEntity.ShadowCardNumber = GenerateShadowCardNumber();
        //    //cardNumberEntity.CardRefNumber = GenerateCardRefNumber();

        //    return cardNumberEntity;
        //}

        private static long GetPaymentSequence()
        {
            //CardNumberDetailOperation op = new CardNumberDetailOperation();
            //CardNumberDetail cardNumberDetail = op.GetCardNumberDetailAll()[0];
            ////cardNumberDetail boş olmamalı, boş olamaz patlasın hatta.
            //if (string.IsNullOrEmpty(cardNumberDetail.LastBarcodeDate))
            //cardNumberDetail.LastBarcodeDate = DateTime.MinValue.ToString("yyyyMMdd");

            //if (DateTime.ParseExact(cardNumberDetail.LastBarcodeDate, "yyyyMMdd", null) < DateTime.Now.Date)
            //{
            //    SequenceManager.Restart(SEQUENCE_BARCODE);
            //    cardNumberDetail.LastBarcodeDate = DateTime.Now.ToString("yyyyMMdd");
            //    op.ModifyCardNumberDetail(cardNumberDetail);

            //}
            return SequenceManager.GetNextPaymentValue();
        }

        private static long GetCustomerSequence()
        {
            return SequenceManager.GetNextCustomerValue();
        }
    }
}
