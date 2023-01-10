using Francisvac.Result;

namespace ResultSample.Services;
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class WeatherForcastService
{
    private readonly string[] summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"  };

    public Result<WeatherForecast[]> GetWeatherForecast(int quantity)
    {
        if (quantity <= 0) return Result.Error("The quantity should be greater than 0");

        var forecast = Enumerable.Range(1, quantity).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();

        return forecast;
    }
}
