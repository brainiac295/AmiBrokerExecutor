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
            Console.WriteLine("AmiBroker Executor For Upstox");
            Console.WriteLine("Updated As of 20/OCT/2020 At 12:36:00 PM");
            Console.WriteLine(DateTime.Now.ToString());
            string Type = args[1];
            
            RestClient Clinet = new RestClient("https://counter20200901203755.azurewebsites.net/api/IndexPlacer?type=" + Type);
            RestRequest Request = new RestRequest(Method.GET);

            Console.WriteLine(DateTime.Now.ToLongTimeString());

            Clinet.Execute(Request);

            Console.WriteLine("Done");
            Console.WriteLine(DateTime.Now.ToLongTimeString());            
        }
    }
}
