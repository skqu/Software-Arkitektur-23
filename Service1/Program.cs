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
            .WithClientId("Service")
            .WithTcpServer("localhost", 1884) // make sure broker listens on 1884
            .Build();

        // Handle received messages (MAKE THIS ASYNC)
        mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            Console.WriteLine($"Subscribed: \"{payload}\"");

            // Republish to another topic
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("test/service2")
                .WithPayload("Received from broker event: " + payload)
                .Build();

            await mqttClient.PublishAsync(message).ConfigureAwait(false);
            Console.WriteLine($"Publish: \"{payload}\"");
        };

        // On connect, subscribe to the input topic
        mqttClient.ConnectedAsync += async e =>
        {
            await mqttClient.SubscribeAsync("test/topic").ConfigureAwait(false);
            Console.WriteLine("Subscribed to test/topic (waiting for messages)...");
        };

        await mqttClient.ConnectAsync(options).ConfigureAwait(false);

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
