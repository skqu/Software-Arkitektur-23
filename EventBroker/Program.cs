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
            .WithClientId("EventBroker")
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
            await mqttClient.SubscribeAsync("test/topic");

            // Publish
            var messageText = "Hello from C#!";
            while (messageText != "q")
            {
                messageText = Console.ReadLine();
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("test/topic")
                    .WithPayload(messageText)
                    .Build();

                await mqttClient.PublishAsync(message);
                Console.WriteLine($"Publish: \"{messageText}\"");
            }
        }
            ;

        await mqttClient.ConnectAsync(options);

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
