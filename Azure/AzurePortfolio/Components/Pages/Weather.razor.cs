using Microsoft.AspNetCore.Components;
using System.Linq;

namespace AzurePortfolio.Components.Pages;

/// <summary>
/// Code-behind for the Weather page component.
/// </summary>
public partial class Weather : ComponentBase
{
    private WeatherForecast[]? forecasts;

    /// <summary>
    /// Initializes the component and loads weather forecast data.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Simulate asynchronous loading to demonstrate streaming rendering
        await Task.Delay(500);

        DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
        string[] summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        }).ToArray();
    }

    /// <summary>
    /// Represents a weather forecast entry.
    /// </summary>
    private class WeatherForecast
    {
        /// <summary>
        /// Gets or sets the date of the forecast.
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Gets or sets the temperature in Celsius.
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// Gets or sets the summary description.
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// Gets the temperature in Fahrenheit.
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}

