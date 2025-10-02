public class Program
{
    public static void Main()
    {
        Console.WriteLine("Service 2 is running");
    }
}


namespace Service2
{
    public class OrderShipment
    {

        public string Currier(string invoice)
        {

            return invoice + " Data from service 2";
        }
    }
}

