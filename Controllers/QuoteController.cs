using EmployeeManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]  // Add this to make sure it's treated as an API controller
    public class QuoteController : ControllerBase
    {
        public QuoteController(IHttpClientFactory httpClientFactory)
        {
        }

        [HttpGet("daily")]
        public async Task<ActionResult> GetQuote()
        {
            string apiUrl = "https://zenquotes.io/api/today";

            // Use IHttpClientFactory to get an instance of HttpClient
            var client = new HttpClient();

            try
            {
                // Fetch the data from the API
                string response = await client.GetStringAsync(apiUrl);

                // Deserialize the response to a list of Quote objects
                var quotes = JsonConvert.DeserializeObject<List<Quote>>(response);

                // Extract the 'h' (HTML-formatted) part of the first quote
                string hPart = quotes[0].h;

                // Return the HTML-formatted quote in the response
                return Ok(hPart);
            }
            catch (Exception ex)
            {
                // Return a server error response in case of failure
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
