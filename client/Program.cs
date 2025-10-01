using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

string serviceName = "client";
var serverUrl = "ws://localhost:5000/ws?name=Client";

var jsonOpts = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

using var ws = new ClientWebSocket();
await ws.ConnectAsync(new Uri(serverUrl), CancellationToken.None);
Console.WriteLine($"Connected as {serviceName}");


var hello = new ChatMessage
{
    Timestamp = DateTimeOffset.UtcNow,
    Sender = serviceName,
    Receiver = "Server",
    Message = "Hello, world!"
};
await SendJson(ws, hello, jsonOpts);

string input = "";

while (input != "q!")
{
    input = Console.ReadLine();
    hello.Message = input;
    await SendJson(ws, hello, jsonOpts);

    // Read a single reply (you can loop if you want continuous ping-pong)
    var reply = await ReceiveJson<ChatMessage>(ws, jsonOpts);
    Console.WriteLine($"[{reply.Timestamp:u}] {reply.Sender} -> {reply.Receiver}: {reply.Message}");

}

await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);

static async Task SendJson<T>(ClientWebSocket ws, T obj, JsonSerializerOptions opts)
{
    var bytes = JsonSerializer.SerializeToUtf8Bytes(obj, opts);
    await ws.SendAsync(bytes, WebSocketMessageType.Text, endOfMessage: true, CancellationToken.None);
}

static async Task<T> ReceiveJson<T>(ClientWebSocket ws, JsonSerializerOptions opts)
{
    var buffer = new byte[4096];
    var ms = new MemoryStream();
    WebSocketReceiveResult result;
    do
    {
        result = await ws.ReceiveAsync(buffer, CancellationToken.None);
        if (result.MessageType == WebSocketMessageType.Close)
            throw new Exception("Closed by server");

        ms.Write(buffer, 0, result.Count);
    } while (!result.EndOfMessage);

    return JsonSerializer.Deserialize<T>(ms.ToArray(), opts)!;
}

public sealed class ChatMessage
{
    public DateTimeOffset Timestamp { get; set; }
    public string Sender { get; set; } = "";
    public string Receiver { get; set; } = "";
    public string Message { get; set; } = "";
}
