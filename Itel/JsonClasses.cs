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

        public static List<Transaction> DeserializeListTransactions(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Transaction>));
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(json));
            List<Transaction> obj = new List<Transaction>();
            if (stream.Length == 0)
                return obj;
            obj = serializer.ReadObject(stream) as List<Transaction>;

            return obj;
        }

        public static void SerializeListTransactions(List<Transaction> list, string path)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Transaction>));
            FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            serializer.WriteObject(stream, list);
            stream.Flush();
            stream.Close();
        }

    }

    #region Details
    public partial class CardDetail
    {
        public bool status { get; set; }
        public object code { get; set; }
        public object dialog { get; set; }
        public object extra { get; set; }
        public Data data { get; set; }

        public static CardDetail Desirialize(string jsonResponse)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CardDetail));
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(jsonResponse));
            CardDetail obj = serializer.ReadObject(stream) as CardDetail;

            return obj;
        }
    }

    public class Data
    {
        public long version { get; set; }
        public IList<Category> categories { get; set; }
    }

    public class Category
    {
        public IList<Item> items { get; set; }
        public IList<Service> services { get; set; }
        public IList<Application1> applications { get; set; }
        public int id { get; set; }
        public string label1 { get; set; }
        public string label2 { get; set; }
        public string foreground1 { get; set; }
        public string foreground2 { get; set; }
        public string background { get; set; }
        public string icon { get; set; }
    }

    public class Application1
    {
        public Uri url { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public string description { get; set; }
        public object foreground { get; set; }
        public object background { get; set; }
        public int index { get; set; }
        public IList<Field> fields { get; set; }
        public int category { get; set; }
    }

    public class Field
    {
        public int id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public bool required { get; set; }
        public int index { get; set; }
        public string data { get; set; }
    }

    public class Service
    {
        public int id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public string picture { get; set; }
        public string help { get; set; }
        public int category { get; set; }
        public int categoryType { get; set; }
        public IList<Denomination1> denominations { get; set; }
        public object background { get; set; }
        public object foreground { get; set; }
        public bool targeted { get; set; }
        public int index { get; set; }
        public bool showNote { get; set; }
        public IList<Bracket> brackets { get; set; }
    }

    public class Bracket
    {
        public double target { get; set; }
        public string incentive { get; set; }
        public double discount { get; set; }
    }

    public class Denomination1
    {
        public int id { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string percentage { get; set; }
        public TopupAction topupAction { get; set; }
        public VoucherAction voucherAction { get; set; }
        public BillAction billAction { get; set; }
    }

    public class VoucherAction
    {
        public string toggleAction { get; set; }
        public string togglePrintAction { get; set; }
        public string action { get; set; }
        public string printAction { get; set; }
        public int minQuantity { get; set; }
        public int maxQuantity { get; set; }
    }

    public class TopupAction
    {
        public string toggleAction { get; set; }
        public string action { get; set; }
        public string hint { get; set; }
    }
    public partial class BillAction
    {
        public string RetrieveAction { get; set; }
        public string PayAction { get; set; }
        public string Hint { get; set; }
    }
    public class Item
    {
        public int type { get; set; }
        public int id { get; set; }
    }

    #endregion


    #region TransactionsNew
    public class RootTransaction
    {
        public bool status { get; set; }
        public object code { get; set; }
        public object dialog { get; set; }
        public object extra { get; set; }
        public Transactions data { get; set; }

        public static RootTransaction Desirialize(string jsonResponse)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootTransaction));
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(jsonResponse));
            RootTransaction obj = serializer.ReadObject(stream) as RootTransaction;

            return obj;
        }
    }

    public class Transactions
    {
        public Summary summary { get; set; }
        public List<Transaction> details { get; set; }
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
        public string transactionId { get; set; }
        public string service { get; set; }
        //public string picture { get; set; }
        //public string pictureUrl { get; set; }
        public string denomination { get; set; }
        public string serial { get; set; }
        //public string secret { get; set; }
        public string formattedSecret { get; set; }
        //public object phone { get; set; }
        public double amount { get; set; }
        //public object originalAmount { get; set; }
        //public object fees { get; set; }
        //public string validity { get; set; }
        public string expiry { get; set; }
        //public string help { get; set; }
        public string time { get; set; }
        public string date { get; set; }
        //public bool voided { get; set; }
        //public bool fromVoid { get; set; }
        //public int printType { get; set; }
        //public int viewType { get; set; }
    }

    public class Summary
    {
        public List<SettlementSummaryRespons> settlementSummaryResponses { get; set; }
        public string totalSales { get; set; }
    }

    public class SettlementSummaryRespons
    {
        public string date { get; set; }
        public List<Detail> details { get; set; }
    }

    public class Detail
    {
        public string denomination { get; set; }
        public int number { get; set; }
        public string picture { get; set; }
    }
    #endregion
}

