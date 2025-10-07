using System.Text.Json;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

const string URL1 = "http://localhost:5218/weatherforecast";
const string URL2 = "http://localhost:5125/weatherforecast";



var external = new HttpClient();
app.MapGet("/getweather", async () =>
{
    var response1 = await external.GetAsync(URL1);
    var response2 = await external.GetAsync(URL2);

    response1.EnsureSuccessStatusCode();
    response2.EnsureSuccessStatusCode();

    var string1 = await response1.Content.ReadAsStringAsync();
    var string2 = await response2.Content.ReadAsStringAsync(); 

    var data1 = JsonSerializer.Deserialize<object>(string1);
    var data2 = JsonSerializer.Deserialize<object>(string2);


    return Results.Ok(new {Data1 = data1, Data2 = data2});
});


var httpClient = new HttpClient();
app.MapGet("/externaldata", async () =>
{
    var response1 = await httpClient.GetAsync(URL1);
    var response2 = await httpClient.GetAsync(URL2);
    response1.EnsureSuccessStatusCode();
 
    var responseData1 = await response1.Content.ReadAsStringAsync();
    var responseData2 = await response2.Content.ReadAsStringAsync();
    var data1 = JsonSerializer.Deserialize<object>(responseData1);
    var data2 = JsonSerializer.Deserialize<object>(responseData2);
 
    return Results.Ok(new { Data1 = data1, Data2 = data2 });
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
