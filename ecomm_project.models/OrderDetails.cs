using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ecomm_project.models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader OrderHeader { get; set; }
        public int productId{ get; set; }
        [ForeignKey("productId")]
        public product Product { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}
