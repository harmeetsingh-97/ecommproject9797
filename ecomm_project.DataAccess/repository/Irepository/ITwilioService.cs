using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_project.DataAccess.repository.Irepository
{
    public interface ITwilioService
    {
        Task SendOrderConfirmationSmsAsync(string toPhoneNumber, int orderId, string customerName, IEnumerable<string> productNames);
        Task MakeOrderConfirmationCallAsync(string toPhoneNumber, int orderId, string customerName, IEnumerable<string> productNames);
        Task MakeOrderConfirmationWhatsAppAsync(string toPhoneNumber, int orderId, string customerName, IEnumerable<string> productNames);
    }
}
