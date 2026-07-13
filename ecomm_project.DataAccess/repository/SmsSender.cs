using ecomm_project.DataAccess.repository.Irepository;
using ecomm_utility;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ecomm_project.DataAccess.repository
{
    public class SmsSender : ISmsSender
    {
        private readonly ITwilioRestClient _client;
        private readonly TwilioSettings _settings;

        public SmsSender(ITwilioRestClient client, IOptions<TwilioSettings> settings)
        {
            _client = client;
            _settings = settings.Value;
        }
        public async Task SendSmsAsync(string number, string message)
        {
            if (!number.StartsWith("+"))
            {
                number = "+91" + number;
            }

            await MessageResource.CreateAsync(
                to: new PhoneNumber(number),
                from: new PhoneNumber(_settings.PhoneNumber),
                body: message,
                client: _client
            );
        }
    }
}
