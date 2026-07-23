import { Component, OnInit } from '@angular/core';

interface WeatherForecast {
  date: Date;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent implements OnInit {
  forecasts: WeatherForecast[] | null = null;
  private summaries: string[] = [
    'Freezing', 'Bracing', 'Chilly', 'Cool', 'Mild', 
    'Warm', 'Balmy', 'Hot', 'Sweltering', 'Scorching'
  ];

  /**
   * Initializes the component and loads weather forecast data.
   */
  async ngOnInit(): Promise<void> {
    // Simulate asynchronous loading to demonstrate streaming rendering
    await this.delay(500);

    const startDate = new Date();
    this.forecasts = Array.from({ length: 5 }, (_, index) => {
      const date = new Date(startDate);
      date.setDate(date.getDate() + index + 1);
      
      return {
        date: date,
        temperatureC: Math.floor(Math.random() * 75) - 20,
        temperatureF: 0,
        summary: this.summaries[Math.floor(Math.random() * this.summaries.length)]
      };
    });

    // Calculate Fahrenheit
    this.forecasts.forEach(forecast => {
      forecast.temperatureF = 32 + Math.floor(forecast.temperatureC / 0.5556);
    });
  }

  /**
   * Delays execution for the specified number of milliseconds.
   */
  private delay(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  /**
   * Formats a date to short date string.
   */
  formatDate(date: Date): string {
    return date.toLocaleDateString();
  }
}
