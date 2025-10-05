using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var factory = new MqttFactory();
        var mqttClient = factory.CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
            .WithClientId("Service 2")
            .WithTcpServer("localhost", 1884) // assumes Mosquitto is running locally
            .Build();

        // Handle received messages
        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            Console.WriteLine($"Subscribed: \"{payload}\"");
 
            return Task.CompletedTask;
        };

        mqttClient.ConnectedAsync += async e =>
        {
            // Subscribe
            await mqttClient.SubscribeAsync("test/service2");
        }
            ;

        await mqttClient.ConnectAsync(options);

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
