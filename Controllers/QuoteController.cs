using EmployeeManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    public class QuoteController : ControllerBase
    {
        public QuoteController(IHttpClientFactory httpClientFactory)
        {
        }

        [HttpGet("daily")]
        public async Task<ActionResult> GetQuote()
        {
            string apiUrl = "https://zenquotes.io/api/today";

            var client = new HttpClient();

            try
            {
                string response = await client.GetStringAsync(apiUrl);

                var quotes = JsonConvert.DeserializeObject<List<Quote>>(response);

                string hPart = quotes[0].h;

                return Ok(hPart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
