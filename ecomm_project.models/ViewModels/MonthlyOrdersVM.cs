using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_project.models.ViewModels
{
    public class MonthlyOrdersVM
    {
        public string SelectedMonth { get; set; }
        public Dictionary<string, List<OrderHeader>> MonthlyOrders {  get; set; }
    }
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate {  get; set; }
        public string Name { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
