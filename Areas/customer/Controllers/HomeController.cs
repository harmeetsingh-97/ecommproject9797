using ecomm_project.DataAccess.Repository.Irepository;
using ecomm_project.models;
using ecomm_project.models.ViewModels;
using ecomm_utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics;
using System.Security.Claims;


namespace ecomm_project.Areas.customer.Controllers
{
    [Area("customer")]
    
    public class HomeController : Controller
    {
        private readonly iunitofwork _unitofwork;
        public HomeController(iunitofwork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims != null)
            {
                var count = _unitofwork.shoppingCart.GetAllOrders(sc => sc.ApplicationUserId == claims.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.SS_CartSessionCount, count);
            }
            var productlist = _unitofwork.product.GetAllOrders();
            return View(productlist);
        }
        
        public IActionResult Details(int id)
        {
            var productindb = _unitofwork.product.FirstOrDefault(p => p.id == id,
                includeproperties: "category,covertype");
            if (productindb == null) return NotFound();
            var shoppingcart = new ShoppingCart()
            {
                product = productindb,
                ProductId = id
            };
            return View(shoppingcart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.id = 0;
            if(ModelState.IsValid)
            {
                var claimsidentity = (ClaimsIdentity)(User.Identity);
                var claims = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
                if (claims == null) return NotFound();
                shoppingCart.ApplicationUserId = claims.Value;
                var shoppingcartindb = _unitofwork.shoppingCart.FirstOrDefault
                (sc => sc.ApplicationUserId == claims.Value && sc.ProductId == shoppingCart.ProductId);
                if (shoppingcartindb == null)
                _unitofwork.shoppingCart.Add(shoppingCart);
                else
                shoppingcartindb.count += shoppingCart.count;
                _unitofwork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                var productindb = _unitofwork.product.FirstOrDefault(P => P.id == shoppingCart.id, includeproperties:
               "category,covertype");
                if (productindb == null) return NotFound();
                var shoppingcart = new ShoppingCart()
                {
                    product = productindb,
                    ProductId = shoppingCart.id
                };
            }
            return View(shoppingCart);

        }
       
        public IActionResult Privacy()
        {
             return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
