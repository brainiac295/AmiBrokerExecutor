using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;


namespace AmiBrokerExecutor
{
    class Program
    {
        public static string APIKey = "";
        public static string OrderUrl = "";
        public static string QueryUrl = "";
        public static int CurrencyQTY = 1000;
        public static int IndexQty = 25;
        public static string OptionStartStr = "BANKNIFTY20917";
        public static string BANKNIFTYFUT = "BANKNIFTY20SEPFUT";

        static void Main(string[] args)
        {
            Console.WriteLine("AmiBroker Executor For Upstox");
            string Type = args[3];
            string Line = "";

            string[] Files = Directory.GetFiles(Directory.GetCurrentDirectory());
            foreach (string F in Files)
            {
                if (F.Contains(".txt"))
                {
                    int StartIndex = F.LastIndexOf("\\");
                    StartIndex++;
                    string Str = F.Substring(StartIndex);
                    Str = Str.Replace(".txt", "");
                    Line = Str;
                    break;
                }
            }

            Console.WriteLine("Token:" + Line);


            if (args[0].Contains("USDINR") || args[0].Contains("INRUSD"))
            {
                if (Type == "ShortSell")
                {
                    PlaceOrder(Line, (CurrencyQTY * 2), "s");
                }
                else
                {
                    PlaceOrder(Line, (CurrencyQTY * 2), "b");
                }
                
            } else if (args[0].Contains("BANKNIFTY"))
            {
                RestClient restClient = new RestClient("https://api.upstox.com/live/feed/now/nse_fo/" + BANKNIFTYFUT+"/ltp");
                RestRequest request = new RestRequest(Method.GET);
                request.AddHeader("authorization", "Bearer " + Line);
                request.AddHeader("x-api-key", APIKey);

                var Res = restClient.Execute(request);
                JObject Content = JObject.Parse(Res.Content);
                float LTP = Convert.ToSingle(Content["data"]["ltp"].ToString());
                if (Type=="ShortSell")
                {
                    PlaceOrder(Line, IndexQty, "s", BANKNIFTYFUT);
                    LTP = LTP / 100;
                    LTP = (int)LTP;
                    LTP = LTP * 100;
                    LTP += 300;
                    string OptionToSell = OptionStartStr + LTP.ToString() + "CE";
                    RestClient NewC = new RestClient("https://api.upstox.com/live/feed/now/nse_fo/" + OptionToSell + "/ltp");
                    RestRequest LTPRequest = new RestRequest(Method.GET);

                    LTPRequest.AddHeader("authorization", "Bearer " + Line);
                    LTPRequest.AddHeader("x-api-key", APIKey);

                    var Resp = NewC.Execute(LTPRequest);
                    JObject result = JObject.Parse(Resp.Content);
                    float Price = Convert.ToSingle(result["data"]["ltp"].ToString());
                    PlaceOrder(Line, IndexQty, "s", OptionToSell,Price);
                    
                } else
                {
                    PlaceOrder(Line, IndexQty, "b", BANKNIFTYFUT);
                    LTP = LTP / 100;
                    LTP = (int)LTP;
                    LTP = LTP * 100;
                    LTP -= 300;
                    string OptionToSell = OptionStartStr + LTP.ToString() + "PE";
                    RestClient NewC = new RestClient("https://api.upstox.com/live/feed/now/nse_fo/" + OptionToSell + "/ltp");
                    RestRequest LTPRequest = new RestRequest(Method.GET);

                    LTPRequest.AddHeader("authorization", "Bearer " + Line);
                    LTPRequest.AddHeader("x-api-key", APIKey);

                    var Resp = NewC.Execute(LTPRequest);
                    JObject result = JObject.Parse(Resp.Content);
                    float Price = Convert.ToSingle(result["data"]["ltp"].ToString());
                    PlaceOrder(Line, IndexQty, "b", OptionToSell, Price);
                }
            }
        }

        public static void PlaceOrder(string Line,int QTY,string Type)
        {
            RestClient restClient = new RestClient("https://api.upstox.com/live/orders");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "Bearer " + Line);
            request.AddHeader("x-api-key", APIKey);

            JObject NewObject = new JObject();
            NewObject.Add("transaction_type", Type);
            NewObject.Add("exchange", "ncd_fo");
            NewObject.Add("symbol", "USDINR20SEPFUT");
            NewObject.Add("quantity", QTY);
            NewObject.Add("order_type", "m");
            NewObject.Add("product", "I");

            request.AddJsonBody(NewObject.ToString());
            restClient.Execute(request);
        }

        public static void PlaceOrder(string Line,int Qty,string Type,string Future)
        {
            RestClient restClient = new RestClient("https://api.upstox.com/live/orders");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "Bearer " + Line);
            request.AddHeader("x-api-key", APIKey);

            JObject NewObject = new JObject();
            NewObject.Add("transaction_type", Type);
            NewObject.Add("exchange", "nse_fo");
            NewObject.Add("symbol", Future);
            NewObject.Add("quantity", Qty);
            NewObject.Add("order_type", "m");
            NewObject.Add("product", "I");

            request.AddJsonBody(NewObject.ToString());
            restClient.Execute(request);
        }

        public static void PlaceOrder(string Line,int Qty,string Type,string OptionStr,float Price)
        {
            RestClient restClient = new RestClient("https://api.upstox.com/live/orders");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "Bearer " + Line);
            request.AddHeader("x-api-key", APIKey);

            JObject NewObject = new JObject();
            NewObject.Add("transaction_type", Type);
            NewObject.Add("exchange", "nse_fo");
            NewObject.Add("symbol", OptionStr);
            NewObject.Add("quantity", Qty);
            NewObject.Add("order_type", "l");
            if (Type == "s")
                NewObject.Add("price", (Price - 5));
            else
                NewObject.Add("price", (Price + 5));
            NewObject.Add("product", "I");

            request.AddJsonBody(NewObject.ToString());
            restClient.Execute(request);
        }
    }
}
