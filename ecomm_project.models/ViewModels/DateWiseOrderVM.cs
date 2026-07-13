using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_project.models.ViewModels
{
    public class DateWiseOrderViewModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Status { get; set; }
        public List<OrderHeader> Orders { get; set; }
        public List<OrderHeader> OrderHeader { get; set; }
    }
}
