using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Itel
{
    public class RootObject<T>
    {
        public bool status { get; set; }
        public object code { get; set; }
        public object dialog { get; set; }
        public object extra { get; set; }
        public T data { get; set; }
    }

    #region Balance Classes
    public class BalanceData
    {
        public BalanceDetails balanceDetails { get; set; }
        public object responseDenominations { get; set; }
        public double balance { get; set; }
        public double voidBalance { get; set; }
    }

    public class BalanceDetails
    {
        public String balance { get; set; }
        public String voidBalance { get; set; }
    }
    #endregion

    #region Services Classes

    public class ServiceData
    {
        public int id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public string picture { get; set; }
        public int category { get; set; }
        public List<Denomination> denominations { get; set; }
    }

    public class Denomination
    {
        public int id { get; set; }
        public string name { get; set; }
        public string supportNumber { get; set; }
        public string help { get; set; }
        public double faceValue { get; set; }
        public string validity { get; set; }
        public List<object> actions { get; set; }
        public int type { get; set; }
    }
    #endregion

    #region Sales Items Classes

    [DataContract]
    public class SalesData
    {
        [DataMember(Name = "listOfSoldItems")]
        public List<Voucher> Vouchers { get; set; }
    }

    public class Voucher
    {
        public string date { get; set; }
        public int transactionId { get; set; }
        public string service { get; set; }
        public string denomination { get; set; }
        public double denominationValue { get; set; }
        public string validityPeriod { get; set; }
        public string expiryDate { get; set; }
        public int itemId { get; set; }
        public int sessionCounter { get; set; }
        public string serialNumber { get; set; }
        public string secretCode { get; set; }
        public string help { get; set; }
        public string contact { get; set; }
        public int type { get; set; }
        public string picture { get; set; }
        public int category { get; set; }
    }
    #endregion

    public class Json
    {
        public static RootObject<T> Desirialize<T>(string jsonResponse)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject<T>));
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(jsonResponse));
            RootObject<T> obj = serializer.ReadObject(stream) as RootObject<T>;

            return obj;
        }
    }
    #region Transaction
    public class RootTransaction
    {
        public bool status { get; set; }
        public object code { get; set; }
        public object dialog { get; set; }
        public object extra { get; set; }
        public List<Transaction> data { get; set; }

        public static RootTransaction Desirialize(string jsonResponse)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootTransaction));
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(jsonResponse));
            RootTransaction obj = serializer.ReadObject(stream) as RootTransaction;

            return obj;
        }
    }

    public class Transaction
    {
        public string date { get; set; }
        public List<MyLogInvoiceResponse> myLogInvoiceResponses { get; set; }
    }

    public class MyLogInvoiceResponse
    {
        public bool batch { get; set; }
        public int count { get; set; }
        public int batch_id { get; set; }
        public string date { get; set; }
        public List<LogDetail> logDetails { get; set; }
    }

    public class LogDetail
    {
        public object bytes { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string secretNumber { get; set; }
        public string transactionId { get; set; }
        public string secretCode { get; set; }
        public string service { get; set; }
        public string help { get; set; }
        public int category { get; set; }
        public string description { get; set; }
        public double amount { get; set; }
        public string picture { get; set; }
        public string validityPeriod { get; set; }
        public string expiryDate { get; set; }
        public string directTopupPhone { get; set; }
        public int type { get; set; }
        public int counter { get; set; }
        public bool voided { get; set; }
        public string contact { get; set; }
        public bool fromVoid { get; set; }
        public double additionalFees { get; set; }
        public string shortCode { get; set; }
    }

    #endregion

    #region Details
    public partial class CardDetail
    {
        public Datum[] data { get; set; }

        public object extra { get; set; }

        public object code { get; set; }

        public object dialog { get; set; }

        public bool status { get; set; }

        public static CardDetail Desirialize(string jsonResponse)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CardDetail));
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(jsonResponse));
            CardDetail obj = serializer.ReadObject(stream) as CardDetail;

            return obj;
        }
    }

    public partial class Datum
    {
        public Denomination1[] denominations { get; set; }

        public long category { get; set; }

        public Brackets[] brackets { get; set; }

        public double current { get; set; }

        public string name { get; set; }

        public long id { get; set; }

        public string picture { get; set; }

        public long type { get; set; }
    }

    public partial class Denomination1
    {
        public long id { get; set; }

        public TopupAction topupAction { get; set; }

        public double faceValue { get; set; }

        public object billAction { get; set; }

        public string help { get; set; }

        public double price { get; set; }

        public string name { get; set; }

        public string supportNumber { get; set; }

        public string validity { get; set; }

        public long type { get; set; }

        public VoucherAction voucherAction { get; set; }
    }

    public partial class TopupAction
    {
        public string hint { get; set; }

        public string action { get; set; }

        public string toggleAction { get; set; }
    }

    public partial class VoucherAction
    {
        public long maxQuantity { get; set; }

        public string printAction { get; set; }

        public string action { get; set; }

        public long minQuantity { get; set; }

        public string toggleAction { get; set; }

        public string togglePrintAction { get; set; }
    }

    public partial class Brackets
    {
        public string incentive { get; set; }

        public double discount { get; set; }

        public long target { get; set; }
    }
    #endregion
}

