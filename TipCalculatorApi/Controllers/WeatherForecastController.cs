using Microsoft.AspNetCore.Mvc;

namespace TipCalculatorApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [ApiController]
    [Route("[controller]")]
    public class TipController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult<Dictionary<string, double>> CalculateTip([FromBody] Bill bill)
        {
            if (bill == null || bill.Amount <= 0)
            {
                return BadRequest("Invalid bill amount.");
            }

            var tipPercentages = new List<double> { 0.18, 0.20, 0.22 };
            var results = new Dictionary<string, double>();

            foreach (var percentage in tipPercentages)
            {
                var tipAmount = bill.Amount * percentage;
                results.Add($"Tip ({percentage * 100}%)", tipAmount);
                results.Add($"Total with {percentage * 100}% tip", bill.Amount + tipAmount);
            }

            // Save to the database if needed
            _context.Bills.Add(bill);
            _context.SaveChanges();

            // Return the calculated tip and total amounts
            return results;
        }

    }
}
