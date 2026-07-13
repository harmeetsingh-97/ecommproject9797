using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_utility
{
    public static class SD
    {
        //roles
        public const string role_Admin = "Admin User";
        public const string role_Employee = "Employee User";
        public const string role_Company = "Company User";
        public const string role_Individual = "Individual User";

        //session
        public const string SS_CartSessionCount = "Cart Count Session";
        //session cart count 
        public static double GetPriceBasgetedOnQuantity(double count, double price,double price50,double price100)
        {
            if (count < 50)
                return price;
            else if(count < 100)
                return price50; 
            return price100;
        }
        //order status
        public const string orderstatusPending = "Pending";
        public const string orderstatusApproved = "Approved";
        public const string orderstatusInProgress = "Processing";
        public const string orderstatusShipped = "Shipped";
        public const string orderstatusCancelled = "Cancelled";
        public const string orderstatusRefunded = "Refunded";
        //payment status
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayPayment = "Payment Status Delay";
        public const string PaymentStatusRejected = "Rejected";
        //cash on deleivery
        public const string PaymentStatusCOD = "CashOnDelivery";
    }
}
