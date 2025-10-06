using System;
using System.Text;
using MQTTnet;
using MQTTnet.Client;

class Program
{
    static async Task Main()
    {
        var factory = new MqttFactory();
        var mqttClient = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithClientId("Service 3")
            .WithTcpServer("localhost", 1884)
            .Build();

        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            Console.WriteLine(payload);
            return Task.CompletedTask;
        };

        mqttClient.ConnectedAsync += async e =>
        {
            await mqttClient.SubscribeAsync("test/topic");
        };

        await mqttClient.ConnectAsync(options);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

    }
}