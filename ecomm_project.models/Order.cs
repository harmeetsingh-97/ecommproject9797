using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_project.models
{
    public enum PaymentMethod { Stripe,COD}
    public enum PaymentStatus { Pending,Completed,Failed}
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public bool IsConfirmed { get; set; }//used for cod confirmation
    }
}
