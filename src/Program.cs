using System.Net.Http.Headers;
using System.Text.Json;
using System.Diagnostics; 

var builder = WebApplication.CreateBuilder(args);

// NEVER hardcode secrets in real apps
var clientId = "Ov23lixzOw1qmCRKsLxD";
var clientSecret = "03f481b49772bc72ed6d5d0806a9987f2293c042";
var redirectUri = "http://localhost:5066/callback";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost") 
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

string? accessToken = null;

app.MapGet("/login", () =>
{
    var authUrl =
        $"https://github.com/login/oauth/authorize?client_id={clientId}" +
        $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
        $"&scope={Uri.EscapeDataString("read:user user:email")}";
    Process.Start(new ProcessStartInfo(authUrl) { UseShellExecute = true });
});

app.MapGet("/callback", async (HttpContext ctx, IHttpClientFactory factory) =>
{
    var code = ctx.Request.Query["code"].ToString();
    if (string.IsNullOrEmpty(code))
        return Results.BadRequest("Missing 'code' query parameter.");

    var http = factory.CreateClient();
    http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var form = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        ["client_id"] = clientId,
        ["client_secret"] = clientSecret,
        ["code"] = code,
        ["redirect_uri"] = redirectUri
    });

    var resp = await http.PostAsync("https://github.com/login/oauth/access_token", form);
    if (!resp.IsSuccessStatusCode)
        return Results.Problem($"Token endpoint returned {(int)resp.StatusCode}");

    using var stream = await resp.Content.ReadAsStreamAsync();
    var doc = await JsonDocument.ParseAsync(stream);
    accessToken = doc.RootElement.TryGetProperty("access_token", out var tok) ? tok.GetString() : null;

    if (string.IsNullOrEmpty(accessToken))
        return Results.Problem("No access_token in response");

    await ctx.Response.WriteAsync("<html><body>Login complete. You can close this window.</body></html>");
    return Results.Empty;
});


app.MapGet("/token", async (IHttpClientFactory httpFactory) =>
{
    if (string.IsNullOrEmpty(accessToken))
    {
        return Results.Unauthorized();
    }

    var http = httpFactory.CreateClient();
    http.DefaultRequestHeaders.UserAgent.ParseAdd("your-app/1.0");
    http.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

    var resp = await http.GetAsync("https://api.github.com/user");

    if (!resp.IsSuccessStatusCode)
    {
        return Results.Unauthorized();
    }

    var userJson = await resp.Content.ReadAsStringAsync();
    var user = JsonSerializer.Deserialize<JsonElement>(userJson);

    return Results.Ok(new
    {
        access_token = accessToken,
        user 
    });
});


app.Run();
