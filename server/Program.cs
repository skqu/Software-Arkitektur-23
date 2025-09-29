using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Urls.Add("http://localhost:5000");
app.UseWebSockets();

var connections = new ConcurrentDictionary<string, WebSocket>();
var jsonOpts = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = false };

app.Map("/ws", async ctx =>
{
    if (!ctx.WebSockets.IsWebSocketRequest)
    {
        ctx.Response.StatusCode = 400;
        await ctx.Response.WriteAsync("WebSocket request expected");
        return;
    }

    var name = ctx.Request.Query["name"].ToString();
    if (string.IsNullOrWhiteSpace(name)) name = Guid.NewGuid().ToString("N");

    using var socket = await ctx.WebSockets.AcceptWebSocketAsync();
    connections[name] = socket;
    Console.WriteLine($"[{DateTimeOffset.UtcNow:u}] {name} connected.");
    
    try
    {
        var buffer = new byte[4096];

        while (
            socket.State == WebSocketState.Open &&
            !ctx.RequestAborted.IsCancellationRequested)
        {
            var ms = new MemoryStream();
            WebSocketReceiveResult? result;
            do
            {
                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), ctx.RequestAborted);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ctx.RequestAborted);
                    Console.WriteLine($"[{DateTimeOffset.UtcNow:u}] {name} closed.");
                    connections.TryRemove(name, out _);
                    return;
                }
                ms.Write(buffer, 0, result.Count);
            } while (!result.EndOfMessage);

            var json = Encoding.UTF8.GetString(ms.ToArray());
            var incoming = JsonSerializer.Deserialize<ChatMessage>(json, jsonOpts);

            if (incoming is not null)
            {
                Console.WriteLine($"[{DateTimeOffset.UtcNow:u}] {name} received from {incoming.Sender} -> {incoming.Receiver}: {incoming.Message}");
                // Simple “back and forth”: echo a reply to the same socket
                var reply = new ChatMessage
                {
                    Timestamp = DateTimeOffset.UtcNow,
                    Sender = "Server",
                    Receiver = incoming.Sender,
                    Message = $"ACK: {incoming.Message}"
                };

                var payload = JsonSerializer.SerializeToUtf8Bytes(reply, jsonOpts);
                await socket.SendAsync(new ArraySegment<byte>(payload), WebSocketMessageType.Text, endOfMessage: true, ctx.RequestAborted);
            }
        }
    }
    catch (OperationCanceledException) { /* shutting down */ }
    catch (Exception ex)
    {
        Console.WriteLine($"[{DateTimeOffset.UtcNow:u}] Error for {name}: {ex.Message}");
    }
    finally
    {
        connections.TryRemove(name, out _);
        try { await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server closing", CancellationToken.None); } catch { }
    }
});

app.Run();

public sealed class ChatMessage
{
    public DateTimeOffset Timestamp { get; set; }
    public string Sender { get; set; } = "";
    public string Receiver { get; set; } = "";
    public string Message { get; set; } = "";
}
