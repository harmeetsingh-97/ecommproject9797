using ecomm_project.DataAccess.repository.Irepository;
using ecomm_project.models;
using ecomm_utility;
using Microsoft.Extensions.Options;
using System.Runtime;
using Twilio.Clients;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Api.V2010.Account;

using Twilio.TwiML;
using Twilio.TwiML.Messaging;
using Twilio.Types;
namespace ecomm_project.DataAccess.repository
{
    public class TwilioService : ITwilioService
    {
        private readonly ITwilioRestClient _client;
        private readonly TwilioSettings _settings;
        public TwilioService(ITwilioRestClient client, IOptions<TwilioSettings> settings)
        {
            _client = client;
            _settings = settings.Value;
        }
        public async Task SendOrderConfirmationSmsAsync(string toPhoneNumber, int orderId, string customerName, IEnumerable<string> productNames)
        {
            if (!toPhoneNumber.StartsWith("+"))
            {
                toPhoneNumber = "+91" + toPhoneNumber;
            }
            string products = string.Join(",", productNames);

            await MessageResource.CreateAsync(
                body: $"Hi {customerName}, Your order #{orderId} for [{products}] is confirmed",
                from: new PhoneNumber(_settings.PhoneNumber),
                to: new PhoneNumber(toPhoneNumber),
                client: _client
                );
        }
        public async Task MakeOrderConfirmationCallAsync(string toPhoneNumber, int orderld, string customerName, IEnumerable<string> productNames)
        {
            if (!toPhoneNumber.StartsWith("+"))
            {
                toPhoneNumber = "+91" + toPhoneNumber;
                }
                string products = string.Join(". ", productNames);
            
            var response = new VoiceResponse();
            response.Say($"Hello {customerName}. Thank you for shopping with us. ", voice: "Polly.Kajal-Neural", language: "en-IN");
            response.Pause(1);
            response.Say($"Your order number is {orderld}.", voice: "Polly Kajal-Neural", language: "en-IN");
            response.Pause(1);
            response.Say($"You Have Ordered: {products}.We Will Notify you When it ships.Goodbye!", voice: "Polly.Kajal-Neural", language: "en-IN");
            await CallResource.CreateAsync(
                twiml: response.ToString(),
                from: new PhoneNumber(_settings.PhoneNumber),
                to: new PhoneNumber(toPhoneNumber),
                client: _client
                );
        }
        public async Task MakeOrderConfirmationWhatsAppAsync(string toNumber, int orderId, string customerName, IEnumerable<string> productNames)
        {
            if (!toNumber.StartsWith("+"))
            {
                toNumber = "+91" + toNumber; // India code
            }

            string products = string.Join(", ", productNames);

            await MessageResource.CreateAsync(
                body: $"*Hi {customerName}*, Your order #{orderId} for [{products}] is confirmed! ✅",
                from: new Twilio.Types.PhoneNumber("whatsapp:" + _settings.WhatsAppNumber), // Twilio WhatsApp number
                to: new Twilio.Types.PhoneNumber("whatsapp:" + toNumber),               // Receiver's WhatsApp number
                client: _client
            );
        }
    } 
}


