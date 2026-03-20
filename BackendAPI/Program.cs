var builder = WebApplication.CreateBuilder(args);

// 1. Define the CORS policy name
var MyAllowAllOrigins = "_myAllowAllOrigins";

// 2. Add CORS services to allow any frontend to connect
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowAllOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()   
                                .AllowAnyHeader()   
                                .AllowAnyMethod();  
                      });
});

builder.Services.AddOpenApi();

var app = builder.Build();

// 3. Enable CORS in the pipeline - MUST be before MapGet
app.UseCors(MyAllowAllOrigins);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// This is your API endpoint: https://<your-app>.azurewebsites.net/weatherforecast
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}