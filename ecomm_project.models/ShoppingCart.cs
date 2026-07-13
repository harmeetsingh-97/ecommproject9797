using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ecomm_project.models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            count = 1;
        }
        public int id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public product product { get; set; }
        public int count { get; set; }
        [NotMapped]
        public double price { get; set; }

    }
}
