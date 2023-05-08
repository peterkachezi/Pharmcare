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
        public OnlinePaymentController(IPaymentRepository paymentRepository, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;

            this.paymentRepository = paymentRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<string> RegisterUrls()
        {
            try
            {
                var jsonBody = JsonConvert.SerializeObject(new
                {
                    ValidationURL = "https://skisoftsystems.com/payment/validation",

                    ConfirmationURL = "https://skisoftsystems.com/payment/confirmation",

                    ResponseType = "Completed",

                    ShortCode = "174379",
                });

                var jsonReadyBody = new StringContent(

                    jsonBody.ToString(),

                    Encoding.UTF8,

                    "application/json");

                var token = await generateAccessToken();

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
        public async Task<ActionResult> MpesaPayment()
        {
            try
            {
                string url = @"https://sandbox.safaricom.co.ke/mpesa/c2b/v1/registerurl";
                //string url =@"https://sandbox.safaricom.co.ke/mpesa/stkpush/v1/processrequest";


                HttpClient client = new HttpClient();


                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + await generateAccessToken());

                var mpesaExpressRequestDTO = new RegisterURLS
                {
                    ValidationURL = "https://skisoftsystems.com/payment/validation",

                    ConfirmationURL = "https://skisoftsystems.com/payment/confirmation",

                    ResponseType = "Completed",

                    ShortCode = "174379",

                };

                // HttpResponseMessage result = await client.PostAsJsonAsync(url, mpesaExpressRequestDTO);

                string post_params = JsonConvert.SerializeObject(mpesaExpressRequestDTO);

                HttpContent content = new StringContent(post_params, Encoding.UTF8, "application/json");

                HttpResponseMessage result = await client.PostAsync(url, content);

                //result.EnsureSuccessStatusCode();

                var response = await result.Content.ReadAsStringAsync();


                //var mpesaExpressResponse = JsonConvert.DeserializeObject<MpesaExpressResponseDTO>(response);

                //var mpesaResponse = new MpesaExpressResponseDTO
                //{
                //    MerchantRequestID = mpesaExpressResponse.MerchantRequestID,

                //    CheckoutRequestID = mpesaExpressResponse.CheckoutRequestID,

                //    ResponseCode = mpesaExpressResponse.ResponseCode,

                //    ResponseDescription = mpesaExpressResponse.ResponseDescription,

                //    CustomerMessage = mpesaExpressResponse.CustomerMessage,
                //};

                //var h = await SaveResponse(mpesaResponse);

                return Json(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }
        public async Task<string> generateAccessToken()
        {
            try
            {
                var url = @"https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials";

                var key = "MVOoIKyTwGExVIZhHXrYa1nB2Xj0fhQK";

                var secrete = "xpT9QD1UFvMXSgIT";

                HttpClient client = new HttpClient();

                var byteArray = Encoding.ASCII.GetBytes(key + ":" + secrete);

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                HttpResponseMessage response = await client.GetAsync(url);

                HttpContent content = response.Content;

                string result = await content.ReadAsStringAsync();

                Token token = JsonConvert.DeserializeObject<Token>(result);

                return token.access_token;
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

                var authString = "2MtKYV3O4QvE23xoeiruN3vTJZUtDJEI:x5rwJGFtLFTC1K0q";

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

    internal class RegisterURLS
    {
        public string ValidationURL { get; set; }
        public string ConfirmationURL { get; set; }
        public string ResponseType { get; set; }
        public string ShortCode { get; set; }
    }
}
