using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.IO;


namespace AmiBrokerExecutor
{
    class Program
    {        
        static void Main(string[] args)
        {
            Console.WriteLine("AmiBroker Executor For Upstox & Firebase");
            Console.WriteLine("Updated As of 05/NOV/2020 At 06:24:00 PM");
            Console.WriteLine(DateTime.Now.ToString());
            string Type = args[1];
            string Instrument = args[0];

            if (Instrument.Contains("BANKNIFTY"))
            {
                RestClient Clinet = new RestClient("https://dvkorder.azurewebsites.net/api/IndexPlacer?type=" + Type);
                RestRequest Request = new RestRequest(Method.GET);
                Clinet.Execute(Request);
            } else
            {

            }            

            Console.WriteLine(DateTime.Now.ToLongTimeString());

            

            Console.WriteLine("Done");
            Console.WriteLine(DateTime.Now.ToLongTimeString());
            System.Threading.Thread.Sleep(5000);
        }
    }
}
