using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmCare.BLL.Repositories.PaymentModule;
using PharmCare.DAL.Models;
using PharmCare.Models;
using System.Net.Mime;
using System.Security.AccessControl;
using System.Text;

namespace PharmCare.Controllers
{
    public class OnlinePaymentController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        private readonly IPaymentRepository paymentRepository;
        public OnlinePaymentController(IPaymentRepository paymentRepository,IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;

            this.paymentRepository = paymentRepository;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]

        public async Task<string> RegisterUrls()
        {
            try
            {
                var jsonBody = JsonConvert.SerializeObject(new
                {
                    ValidationURL = "https://1e70-154-70-3-144.ap.ngrok.io/api/Payme/Validation",

                    ConfirmationURL = "https://1e70-154-70-3-144.ap.ngrok.io/api/Payme/Payment_Confirmation",

                    ResponseType = "Complted",

                    ShortCode = "600998",
                });

                var jsonReadyBody = new StringContent(

                    jsonBody.ToString(),

                    Encoding.UTF8,

                    "application/json");

                var token = await GetToken();

                var client = _clientFactory.CreateClient("mpesa");

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var url = "/mpesa/c2b/v1/registerurl";

                var response = await client.PostAsync(url, jsonReadyBody);

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
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

        [HttpPost]
        [Route("payment/confirmation")]
        [HttpPost(MediaTypeNames.Application.Json)]
        public async Task<JsonResult> Payment_Confirmation([FromBody] OnlinePayment onlinePayment)
        {
            try
            {
                var respond = new
                {
                    ResponseCode = 0,

                    ResponseDesc = "Processed"
                };

                var createPayment = await paymentRepository.Create(onlinePayment);

                return Json(respond);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        [HttpPost]
        [Route("payment/validation")]
        [HttpPost(MediaTypeNames.Application.Json)]
        public JsonResult Payment_Validation([FromBody] OnlinePayment onlinePayment)
        {
            try
            {
                var respond = new
                {
                    ResponseCode = 0,

                    ResponseDesc = "Processed"
                };

                return Json(respond);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
