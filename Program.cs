﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmiBrokerExecutor
{
    class Program
    {
        public static string APIKey = "";
        public static string OrderUrl = "";
        public static int QTY = 25;


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            PlaceOrder(APIKey, QTY, "s");
        }

        public static void PlaceOrder(string Line,int QTY,string Type)
        {

        }
    }
}
