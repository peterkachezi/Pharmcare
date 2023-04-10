using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmCare.Models;
using System.Diagnostics;

namespace PharmCare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

		private readonly IHttpClientFactory _clientFactory;
		public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;

			_clientFactory = clientFactory;
		}
        public IActionResult Index()
        {
            return View();
        }
		public async Task<string> GetToken()
		{
			try
			{
				var client = _clientFactory.CreateClient("mpesa");

				var authString = "ROkJ0lcXGGvINbjdQGyUIXW9wAIJhzQb:zbLzX0zve4MNvthc";

				var encodedString = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authString));

				var _url = "/oauth/v1/generate?grant_type=client_credentials";

				var request = new HttpRequestMessage(HttpMethod.Get, _url);

				request.Headers.Add("Authorization", $"Basic {encodedString}");

				var response = await client.SendAsync(request);

				var mpesaResponse = await response.Content.ReadAsStringAsync();

				Token token = JsonConvert.DeserializeObject<Token>(mpesaResponse);

				return token.access_token;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				return null;
			}
		}
	}
}