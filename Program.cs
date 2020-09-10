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
        public static int QTY = 1000;


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
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

            if (args.Count()>=4)
            {
                
            }
        }

        public static void PlaceOrder(string Line,int QTY,string Type)
        {

        }
    }
}
