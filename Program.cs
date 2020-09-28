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
        public static string APIKey = "o9ZAnYkbqe9ZJr8D3wLAj4yghBmCBIhG4tnds9s9";
        public static int CurrencyQTY = 2;
        public static int IndexQty = 25;
        public static int StockOptionQTY = 4300;
        public static string StockFutureName = "TATAMOTORS20SEPFUT";
        public static string OptionStartStr = "BANKNIFTY20917";
        public static string BANKNIFTYFUT = "BANKNIFTY20SEPFUT";

        static void Main(string[] args)
        {
            Console.WriteLine("AmiBroker Executor For Upstox");
            Console.WriteLine("Updated As of 17/SEP/2020");
            string Type = args[3];
            string Line = "";

            string[] Files = Directory.GetFiles("C:\\Users\\Nos4A2\\");
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


            #region Obsolete
            /*
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

                    LTP -= 600;
                    string OptionToBuy = OptionStartStr + LTP.ToString() + "PE";
                    RestClient NewC2 = new RestClient("https://api.upstox.com/live/feed/now/nse_fo/" + OptionToBuy + "/ltp");
                    RestRequest LTPGetRequest = new RestRequest(Method.GET);
                    LTPGetRequest.AddHeader("authorization", "Bearer " + Line);
                    LTPGetRequest.AddHeader("x-api-key", APIKey);

                    var Resp2 = NewC2.Execute(LTPGetRequest);
                    JObject Result = JObject.Parse(Resp2.Content);
                    float Price2 = Convert.ToSingle(Result["data"]["ltp"].ToString());
                    PlaceOptionBuy(Line, IndexQty, OptionToBuy, Price2);
                    
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
                    PlaceOrder(Line, IndexQty, "s", OptionToSell, Price);
                    
                    LTP += 600;
                    string OptionToBuy = OptionStartStr + LTP.ToString() + "CE";
                    RestClient NewC2 = new RestClient("https://api.upstox.com/live/feed/now/nse_fo/" + OptionToBuy + "/ltp");
                    RestRequest LTPGetRequest = new RestRequest(Method.GET);
                    LTPGetRequest.AddHeader("authorization", "Bearer " + Line);
                    LTPGetRequest.AddHeader("x-api-key", APIKey);

                    var Resp2 = NewC2.Execute(LTPGetRequest);
                    JObject Result = JObject.Parse(Resp2.Content);
                    float Price2 = Convert.ToSingle(Result["data"]["ltp"].ToString());
                    PlaceOptionBuy(Line, IndexQty, OptionToBuy, Price2);
                }
            } else if (args[0].Contains("SWING"))
            {
                RestClient client = new RestClient("https://api.upstox.com/live/feed/now/nse_fo/" + BANKNIFTYFUT + "/ltp");
                RestRequest request = new RestRequest(Method.GET);
                request.AddHeader("authorization", "Bearer " + Line);
                request.AddHeader("x-api-key", APIKey);


                var IndexFutureResp = client.Execute(request);
                JObject jObject = JObject.Parse(IndexFutureResp.Content);
                float Price = Convert.ToSingle(jObject["data"]["ltp"].ToString());

                Price = (int)Price;
                Price /= 100;
                Price = (int)Price;
                Price *= 100;
                string PutOption = OptionStartStr + (Price - 200).ToString() + "PE";
                string CallOption = OptionStartStr + (Price + 200).ToString() + "CE";

                client = new RestClient("https://api.upstox.com/live/feed/now/nse_fo/" + PutOption + "/ltp");
                RestRequest PutOptionRequest = new RestRequest(Method.GET);
                PutOptionRequest.AddHeader("authorization", "Bearer " + Line);
                PutOptionRequest.AddHeader("x-api-key", APIKey);

                var PutResponse = client.Execute(PutOptionRequest);
                JObject PutPrice = JObject.Parse(PutResponse.Content);
                Price = Convert.ToSingle(PutPrice["data"]["ltp"].ToString());
                Price += 5;

                client = new RestClient("https://api.upstox.com/live/orders");
                RestRequest PutOptionBuyRequest = new RestRequest(Method.POST);
                PutOptionBuyRequest.AddHeader("authorization", "Bearer " + Line);
                PutOptionBuyRequest.AddHeader("x-api-key", APIKey);

                JObject PutOptionData = new JObject();
                PutOptionData.Add("transaction_type", "b");
                PutOptionData.Add("exchange", "nse_fo");
                PutOptionData.Add("symbol", PutOption);
                PutOptionData.Add("quantity", 25);
                PutOptionData.Add("order_type", "l");
                PutOptionData.Add("price", Price);
                PutOptionData.Add("product", "D");

                PutOptionBuyRequest.AddJsonBody(PutOptionData.ToString());

                var PutOptionOrderResponse = client.Execute(PutOptionBuyRequest);

                client = new RestClient("https://api.upstox.com/live/feed/now/nse_fo/" + CallOption + "/ltp");
                RestRequest CallOptionRequest = new RestRequest(Method.GET);
                CallOptionRequest.AddHeader("authorization", "Bearer " + Line);
                CallOptionRequest.AddHeader("x-api-key", APIKey);

                var CallResponse = client.Execute(CallOptionRequest);
                JObject CallPrice = JObject.Parse(CallResponse.Content);
                Price = Convert.ToSingle(CallPrice["data"]["ltp"].ToString());
                Price += 5;

                client = new RestClient("https://api.upstox.com/live/orders");
                RestRequest CallOptionBuyRequest = new RestRequest(Method.POST);
                CallOptionBuyRequest.AddHeader("authorization", "Bearer " + Line);
                CallOptionBuyRequest.AddHeader("x-api-key", APIKey);

                JObject CallOptionData = new JObject();
                CallOptionData.Add("transaction_type", "b");
                CallOptionData.Add("exchange", "nse_fo");
                CallOptionData.Add("symbol", CallOption);
                CallOptionData.Add("quantity", 25);
                CallOptionData.Add("order_type", "l");
                CallOptionData.Add("price", Price);
                CallOptionData.Add("product", "D");

                CallOptionBuyRequest.AddJsonBody(CallOptionData.ToString());

                var CallOptionOrderResponse = client.Execute(CallOptionBuyRequest);

                //Soon To Be Implemented
            }
            */
            #endregion

            RestClient Clinet = new RestClient("https://counter20200901203755.azurewebsites.net/api/IndexPlacer?type="+Type+"&code="+Line);
            RestRequest Request = new RestRequest(Method.GET);

            Clinet.Execute(Request);

            Console.WriteLine("Done");
          
        }


        #region ObsoleteFunctions

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

        public static void PlaceOptionBuy(string Line,int Qty,string OptionStr,float Price)
        {
            RestClient restClient = new RestClient("https://api.upstox.com/live/orders");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "Bearer " + Line);
            request.AddHeader("x-api-key", APIKey);

            JObject NewObject = new JObject();
            NewObject.Add("transaction_type", "b");
            NewObject.Add("exchange", "nse_fo");
            NewObject.Add("symbol", OptionStr);
            NewObject.Add("quantity", Qty);
            NewObject.Add("order_type", "l");
            NewObject.Add("price", (Price + 5));
            NewObject.Add("product", "D");

            request.AddJsonBody(NewObject.ToString());
            restClient.Execute(request);
        }

        #endregion
    }
}
