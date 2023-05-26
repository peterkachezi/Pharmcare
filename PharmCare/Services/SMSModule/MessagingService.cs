using Microsoft.Extensions.Configuration;
using PharmCare.DAL.DbContext;
using PharmCare.DTO.ApplicationUsersModule;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PharmCare.Services.SMSModule
{
    public class MessagingService : IMessagingService
    {
        private readonly IConfiguration config;

        private readonly ApplicationDbContext context;
        public MessagingService(ApplicationDbContext context, IConfiguration config)
        {
            this.config = config;

            this.context = context;
        }
        public async Task<ApplicationUserDTO> usersAccount(ApplicationUserDTO applicationUserDTO)
        {
            try
            {
                var url = "http://167.172.14.50:4002/v1/send-sms";

                var txtMessage = "Dear  " + applicationUserDTO.FirstName

                    + " Welcome to Malela Pharmacy. Below you will find your login details"

                    + ". Your user name is " + applicationUserDTO.Email
                    
                    + " and password : " + applicationUserDTO.Password;

                var key = config.GetValue<string>("SMS_Settings:BongaSMSKey");

                var secrete = config.GetValue<string>("SMS_Settings:BongaSMSSecrete");

                var apiClientID = config.GetValue<string>("SMS_Settings:BongaSMSApiClientID");

                var serviceID = config.GetValue<string>("SMS_Settings:BongaSMSServiceID");

                var msisdn = formatPhoneNumber(applicationUserDTO.PhoneNumber);

                var formContent = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("apiClientID", apiClientID),
                new KeyValuePair<string, string>("secret", secrete),
                new KeyValuePair<string, string>("key", key),
                new KeyValuePair<string, string>("txtMessage", txtMessage),
                new KeyValuePair<string, string>("MSISDN", msisdn),
                new KeyValuePair<string, string>("serviceID", serviceID),
                new KeyValuePair<string, string>("enqueue", "yes"),
            });

                HttpClient client = new HttpClient();

                HttpResponseMessage apiResult = await client.PostAsync(url, formContent);

                apiResult.EnsureSuccessStatusCode();

                var response = await apiResult.Content.ReadAsStringAsync();


                //return new Tuple<bool, TextAlertDTO, string>(true, textAlertDTO, "Text Alert sent successfully!");

                return applicationUserDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }


        public string formatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;

            string formatted = "";

            if (phoneNumber.StartsWith("0"))
                formatted = "+254" + phoneNumber.Substring(1, phoneNumber.Length - 1);

            if (phoneNumber.StartsWith("7"))
                formatted = "+254" + phoneNumber;

            if (phoneNumber.StartsWith("+254"))
                formatted = phoneNumber;

            if (phoneNumber.StartsWith("254"))
                formatted = "+" + phoneNumber;

            return formatted;
        }

        public async Task<ResetPasswordDTO> PasswordResetEmailNotification(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var url = "http://167.172.14.50:4002/v1/send-sms";

                var txtMessage = "Dear  " + resetPasswordDTO.FullName

                    + " Welcome to Malela Pharmacy. Below you will find your login details"

                    + ". Your new password is " + resetPasswordDTO.Password;            

                var key = config.GetValue<string>("SMS_Settings:BongaSMSKey");

                var secrete = config.GetValue<string>("SMS_Settings:BongaSMSSecrete");

                var apiClientID = config.GetValue<string>("SMS_Settings:BongaSMSApiClientID");

                var serviceID = config.GetValue<string>("SMS_Settings:BongaSMSServiceID");

                var msisdn = formatPhoneNumber(resetPasswordDTO.PhoneNumber);

                var formContent = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("apiClientID", apiClientID),
                new KeyValuePair<string, string>("secret", secrete),
                new KeyValuePair<string, string>("key", key),
                new KeyValuePair<string, string>("txtMessage", txtMessage),
                new KeyValuePair<string, string>("MSISDN", msisdn),
                new KeyValuePair<string, string>("serviceID", serviceID),
                new KeyValuePair<string, string>("enqueue", "yes"),
            });

                HttpClient client = new HttpClient();

                HttpResponseMessage apiResult = await client.PostAsync(url, formContent);

                apiResult.EnsureSuccessStatusCode();

                var response = await apiResult.Content.ReadAsStringAsync();


                //return new Tuple<bool, TextAlertDTO, string>(true, textAlertDTO, "Text Alert sent successfully!");

                return resetPasswordDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
