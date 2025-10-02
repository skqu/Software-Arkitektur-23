using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Service 1 is running");
    }
}


namespace Service1
{
    public class OrderPlaced
    {

        public string PlaceOrder()
        {
            return "Invoice Object :D";
        }
    }
}
