using ecomm_project.DataAccess.Repository;
using ecomm_project.DataAccess.Repository.Irepository;
using ecomm_project.models.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecomm_project.Areas.admin.Controllers
{
    [Area("admin")]
    public class AllOrderController : Controller
    {
        private readonly iunitofwork _unitofwork;
        public AllOrderController(iunitofwork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Details(int orderId)
        {
            var orderHeader = _unitofwork.orderHeader
                .FirstOrDefault(o => o.Id == orderId);

            if (orderHeader == null)
            {
                return NotFound();
            }

            var orderDetails = _unitofwork.orderDetails
                .GetAllOrders(u => u.OrderHeaderId == orderId, includeproperties: "Product");

            var orderVM = new OrderVM
            {
                orderHeader = orderHeader,
                orderDetails = orderDetails
            };

            return View(orderVM);
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DateWiseOrders(DateTime? fromDate, DateTime? toDate, string status)
        {
            var model = new DateWiseOrderViewModel()
            {
                FromDate = fromDate,
                ToDate = toDate,
                Status = status
            };

            var query = _unitofwork.orderHeader.GetAllOrders().AsQueryable();

            // Add the filtering logic here
            if (fromDate.HasValue)
            {
                query = query.Where(u => u.OrderDate >= fromDate);
            }
            if (toDate.HasValue)
            {
                query = query.Where(u => u.OrderDate <= toDate);
            }
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(u => u.OrderStatus == status);
            }

            model.Orders = query.ToList();
            return View(model);
        }
        public IActionResult MonthWiseOrder(string filterMonth)
        {
            // 1. Get all orders (including the User so names show up)
            var orders = _unitofwork.orderHeader.GetAllOrders(includeproperties: "ApplicationUser").ToList();

            // 2. Send the selected month back to the view so the dropdown stays on the selection
            ViewData["CurrentFilter"] = filterMonth;

            // 3. Filter the data list itself if a specific month is chosen
            if (!string.IsNullOrEmpty(filterMonth) && filterMonth != "All Months")
            {
                orders = orders.Where(u => u.OrderDate.ToString("MMMM") == filterMonth).ToList();
            }

            return View(orders);
        }


        #region API's
        [HttpGet]
        public IActionResult GetAll()
        {
            var allorder = _unitofwork.orderHeader.GetAllOrders(includeproperties: "ApplicationUser");
            return Json(new { data = allorder });
        }
        #endregion
    }
}
