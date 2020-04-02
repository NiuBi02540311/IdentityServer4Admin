using System;

namespace UserApiService.Models
{
    public class WeatherForecast
    {
        public DateTime Date { get; internal set; }
        public int TemperatureC { get; internal set; }
        public string Summary { get; internal set; }
    }
}