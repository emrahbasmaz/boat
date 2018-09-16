using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boat.Data
{
    public class CommonDefinitions
    {
        public const string APIKEY = "U6l1hKzpZDrgfTaKxpQm3A/6raDiroAI7ueTgMNwhQs=";
        public const string password = "ayyıldız";
        public static string INTERNAL_SYSTEM_ERROR = "999";
        public static string INTERNAL_SYSTEM_VALIDATION_ERROR = "998";
        public static string INTERNAL_SYSTEM_CONNECTION_ERROR = "997";
        public static string INTERNAL_SYSTEM_TIMEOUT_ERROR = "996";
        public static string INTERNAL_SYSTEM_UNKNOWN_ERROR = "995";
        public static string INTERNAL_PASSWORD_ERROR = "994";
        public static string INTERNAL_CARD_INFO_ERROR = "993";
        public static string INTERNAL_TRANSACTION_ERROR = "990";
        public static readonly string SUCCESS = "000";

        #region Messages
        public static string API_KEY_NOT_MATCH = "Api Key Gecersiz.";
        public static string TOKEN_NOT_VALID = "Token Gecersiz.";
        public static string DEVICE_INFORMATION_NOT_FOUND = "Cihaz Bilgisi Eksik.";
        public static string REQUEST_ID_NOT_FOUND = "Request Id Eksik.";
        public static string RESERVATION_ID_NOT_FOUND = "Reservasyon Id Eksik.";
        public static string IDENTIFICATION_ID_NOT_VALID = "TC Id yanlis.";
        public static string PASSWORD_NOT_VALID = "Sifre hatali.";
        public static string CUSTOMER_NOT_FOUND = "Müşteri Bilgisine Ulasilamadi.";
        public static string CUSTOMER_ADDRESS_NOT_FOUND = "Müşteri Adres Bilgisine Ulasilamadi.";
        public static string PAYMENT_TRANSACTION_NOT_FOUND = "Ödeme Bilgisine Ulasilamadi.";
        public static string CUSTOMER_RELATION_NOT_FOUND = "Müşteri'ye ait kişiler Bilgisine Ulasılamadı.";
        public static string CVV_NOT_VALID = "CVV bilgisi Gecersiz.";
        public static string CARD_NUMBER_NOT_VALID = "Kart numarasi Gecersiz.";
        public static string CARD_NAME_NOT_VALID = "Kart adi Gecersiz.";
        public static string CARD_DATE_NOT_VALID = "Kart tarih bilgisi Gecersiz.";
        public static string BOAT_PHOTOS_NOT_FOUND = "Tekne fotografları Bilgisine Ulasılamadı.";
        public static string PHONE_NUMBER_NOT_FOUND = "Telefon numarası Bilgisine Ulasılamadı.";
        public static string INVALID_NAME = "Kullanıcı Isim Bos Girilemez.";
        public static string BOAT_NOT_FOUND = "Tekne bilgisine ulaşilamadi.";
        public static string REGION_NOT_FOUND = "Bölge bilgisine ulaşilamadi.";

        public static string CITY_NOT_FOUND = "Şehir bilgisine ulaşilamadi.";
        public static string DESCRIPTON_NOT_FOUND = "Şehir DETAY bilgisine ulaşilamadi.";
        public static string COUNTRY_NOT_FOUND = "Ülke bilgisine ulaşilamadi.";
        public static string ZIPCODE_NOT_FOUND = "Posta Kodu bilgisine ulaşilamadi.";

        public static string CURRENCY_CODE_IS_NOT_VALID = "Ödeme koduna ulasılamadi.";

        public static string BOAT_CAPACITY_IS_NOT_ENOUGH = "Tekne kapasitesi uygun değil.";




        public static readonly string SUCCESS_MESSAGE = "Basarili Islem.";
        public static readonly string ERROR_MESSAGE = "Hatali Islem.";
        #endregion
    }
}
