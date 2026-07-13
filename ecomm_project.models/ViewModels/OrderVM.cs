using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_project.models.ViewModels
{
    public class OrderVM
    {
        public OrderHeader orderHeader { get; set; }
        public IEnumerable<OrderDetails> orderDetails { get; set; }
    }
}
