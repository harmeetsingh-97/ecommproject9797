using ecomm_project.DataAccess.repository;
using ecomm_project.DataAccess.repository.Irepository;
using ecomm_project.DataAccess.Repository;
using ecomm_project.DataAccess.Repository.Irepository;
using ecomm_project.models;
using ecomm_project.models.ViewModels;
using ecomm_utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using NuGet.Protocol.Core.Types;
using Stripe;
using System.ComponentModel;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;


namespace ecomm_project.Areas.customer.Controllers
{
    [Area("customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly iunitofwork _unitofwork;
        private static bool isEmailConfirm = false;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITwilioService _twilioService;

        public CartController(iunitofwork unitofwork, IEmailSender emailSender, UserManager<IdentityUser> userManager,ITwilioService twilioService)
        {
            _unitofwork = unitofwork;
            _userManager = userManager;
            _emailSender = emailSender;
            _twilioService = twilioService;
        }
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claims = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null)
            {
                ShoppingCartVM = new ShoppingCartVM()
                {
                    ListCart = new List<ShoppingCart>()
                };
                return View(ShoppingCartVM);
            }
            var count = _unitofwork.shoppingCart.GetAllOrders(sc => sc.ApplicationUserId == claims.Value).ToList().Count;
            HttpContext.Session.GetInt32(SD.SS_CartSessionCount);
            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitofwork.shoppingCart.GetAllOrders(sc => sc.ApplicationUserId == claims.Value, includeproperties: "product"),
                OrderHeader = new OrderHeader()
            };
            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitofwork.applicationUser.FirstOrDefault(au => au.Id == claims.Value);
            foreach (var list in ShoppingCartVM.ListCart)
            {
                if (list.product != null)
                {
                    list.price = SD.GetPriceBasgetedOnQuantity(list.count, list.product.Price, list.product.Price50, list.product.Price100);
                    ShoppingCartVM.OrderHeader.OrderTotal += (list.price * list.count);
                    if (list.product.Description.Length > 100)
                    {
                        list.product.Description = list.product.Description.Substring(0, 99) + ".....";
                    }
                }
            }
            //email confirm
            if (!isEmailConfirm)
            {
                ViewBag.EmailMessage = "Email Has Been Sent Verify Your Email";
                ViewBag.EmailCSS = "text-success";
                isEmailConfirm = false;
            }
            else
            {
                ViewBag.EmailMessage = "Email Must Be Confirm For Authorize Customer";
                ViewBag.EmailCSS = "text-danger";
            }

            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPostli()
        {

            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claims = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitofwork.applicationUser.FirstOrDefault(au => au.Id == claims.Value);
            if (user == null)
                ModelState.AddModelError(String.Empty, "Email Empty!!!!");
            else
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
            return RedirectToAction("Index");
        }
        public IActionResult Summary()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claims = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitofwork.shoppingCart.GetAllOrders(sc => sc.ApplicationUserId == claims.Value, includeproperties: "product"),
                OrderHeader = new OrderHeader()
            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitofwork.applicationUser.FirstOrDefault(au => au.Id == claims.Value);
            foreach (var list in ShoppingCartVM.ListCart)
            {
                list.price = SD.GetPriceBasgetedOnQuantity(list.count, list.product.Price, list.product.Price50, list.product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (list.price * list.count);
                if (list.product.Description.Length > 100)
                {
                    list.product.Description = list.product.Description.Substring(0, 99) + ".....";
                }
            }
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.streetaddress;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.state;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.city;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.postalcode;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;


            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]


        public IActionResult Summary(List<int> selectedProducts, string stripeToken)
        {
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader()
            };
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claims = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null) return NotFound();

            //orderheader initialize kiya agr header null hai toh new header create hoga 

            if (ShoppingCartVM.OrderHeader == null)
            {
                ShoppingCartVM.OrderHeader = new OrderHeader();
            }

            // only selected products
            ShoppingCartVM.ListCart = _unitofwork.shoppingCart
                    .GetAllOrders(sc => sc.ApplicationUserId == claims.Value
                    && selectedProducts.Contains(sc.ProductId),
                    includeproperties: "product");

            ShoppingCartVM.OrderHeader.ApplicationUser =
 _unitofwork.applicationUser.FirstOrDefault(au => au.Id == claims.Value);

            var user = ShoppingCartVM.OrderHeader.ApplicationUser;

            ShoppingCartVM.OrderHeader.Name = user.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = user.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = user.streetaddress;
            ShoppingCartVM.OrderHeader.City = user.city;
            ShoppingCartVM.OrderHeader.State = user.state;
            ShoppingCartVM.OrderHeader.PostalCode = user.postalcode;


            ShoppingCartVM.OrderHeader.OrderStatus = SD.orderstatusPending;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claims.Value;

            _unitofwork.orderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitofwork.Save();

            foreach (var list in ShoppingCartVM.ListCart)
            {
                list.price = SD.GetPriceBasgetedOnQuantity(
                    list.count,
                    list.product.Price,
                    list.product.Price50,
                    list.product.Price100);

                ShoppingCartVM.OrderHeader.OrderTotal += (list.price * list.count);

                OrderDetails orderDetails = new OrderDetails()
                {
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    productId = list.ProductId,
                    Price = list.price,
                    Count = list.count
                };

                _unitofwork.orderDetails.Add(orderDetails);
            }

            _unitofwork.Save();

            // remove only selected products from cart
            _unitofwork.shoppingCart.Removerange(ShoppingCartVM.ListCart);
            _unitofwork.Save();

            HttpContext.Session.SetInt32(SD.SS_CartSessionCount, 0);

            // Stripe Payment
            if (stripeToken == null)
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayPayment;
                ShoppingCartVM.OrderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
                ShoppingCartVM.OrderHeader.OrderStatus = SD.orderstatusApproved;
            }
            else
            {
                var Options = new ChargeCreateOptions()
                {
                    Amount = Convert.ToInt32(ShoppingCartVM.OrderHeader.OrderTotal),
                    Currency = "INR",
                    Description = "OrderId:" + ShoppingCartVM.OrderHeader.Id.ToString(),
                    Source = stripeToken
                };

                var Service = new ChargeService();
                Charge charge = Service.Create(Options);

                if (charge.BalanceTransactionId == null)
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                else
                    ShoppingCartVM.OrderHeader.TransactionId = charge.BalanceTransactionId;

                if (charge.Status.ToLower() == "succeeded")
                {
                    ShoppingCartVM.OrderHeader.OrderStatus = SD.orderstatusApproved;
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
                }

                _unitofwork.Save();
            }

            return View(ShoppingCartVM);
        }
        public IActionResult PlaceOrder()
       
        {
            // 1. Pehle logged-in user ki ID nikalein

            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claims = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);

            // 2. User ki Cart list database se fetch karein (kyunki Post mein ye null ho sakti hai)
            ShoppingCartVM.ListCart = _unitofwork.shoppingCart.GetAllOrders(sc => sc.ApplicationUserId == claims.Value, includeproperties: "product");

            // 3. OrderHeader ki zaroori details set karein
            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claims.Value;
            ShoppingCartVM.OrderHeader.PaymentStatus = "Pending"; // Ya jo bhi aapka status hai
            ShoppingCartVM.OrderHeader.OrderStatus = "Pending";

            // 4. Order Total calculate karein
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.price * cart.count);
            }

            // 5. Ab OrderHeader save karein
            _unitofwork.orderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitofwork.Save();

            // 6. Ab OrderDetails (Items) save karein
            foreach (var item in ShoppingCartVM.ListCart)
            {
                OrderDetails orderDetail = new()
                {
                    productId = item.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id, // Primary Key link
                    Price = item.price,
                    Count = item.count
                };
                _unitofwork.orderDetails.Add(orderDetail);
            }
            _unitofwork.Save();

            // 7. Order Confirmation par bhej dein
            return RedirectToAction("OrderConfirmation", new { id = ShoppingCartVM.OrderHeader.Id });
        }    
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var orderHeader = _unitofwork.orderHeader.FirstOrDefault(u => u.Id == id, includeproperties: "ApplicationUser");
            var orderDetail =_unitofwork.orderDetails.GetAllOrders(u => u.OrderHeaderId == id, includeproperties: "Product");

            var ProductNames = orderDetail.Select(u => u.Product.Title).ToList();
            var customerName = orderHeader.Name ?? "Customer";

            if (orderHeader != null)
            {
                string productTableRows = "";
                foreach (var item in orderDetail)
                {
                    productTableRows += $@"
                    <tr>
                    <td style='padding:10px; border-bottom:1px; solid #eee;'>{item.Product.Title} </td>
                    <td style='padding:10px; border-bottom:1px; solid #eee; text-align:center;'>{item.Count}</td>
                    <td style='padding:10px; border-bottom:1px; solid #eee; text-align:right;'> Rs{item.Price}</ td >
                    <td style='padding:10px; border-bottom:1px; solid #eee; text-align:right;'> Rs{item.Price * item.Count}</ td >
                    </tr>";
                }
                string emailHtml = $@"
                <div style='font-family: sans-serif: max-width:600px; border:1px solid #ddd; padding :20px;'>
                <h2 style='color:#2c3e50;'>Order Confirmation </h2>
                <p>Hi {orderHeader.Name},your order <b>#{id}</b> is confirmed!</p>

                 <table style='width:100%; border-collapse:collapse;'>
                 <thead>
                 <tr style='background: #f8f9fa;'>
                 <th style 'text-align:left; padding:10px;'>Product</th>
                 <th style='padding:10px;'>Qty</th>
                 <th style='text-align-right; padding;10px;'>Price</th>
                 <th style='text-align-right; padding;10px;'>Total</th>
                 </tr> 
                 </thead>
                 <tbody>
                 {productTableRows}
                 </tbody>
                 </table>
                 <div style='text-align:right; margin-top:20px;font-size:1.2em;'>
                 <b>Grand Total:Rs{orderHeader.OrderTotal}</b>
                 </div>
                 <p style='margin-top:30px; font-size:0.9em; color:#777;'>Thankyou For Shopping Us!</p>
                 </div>";
                try
                {
                    await _twilioService.SendOrderConfirmationSmsAsync(orderHeader.PhoneNumber, id, customerName, ProductNames);
                    await _twilioService.MakeOrderConfirmationCallAsync(orderHeader.PhoneNumber, id, customerName, ProductNames);
                    await _twilioService.MakeOrderConfirmationWhatsAppAsync(orderHeader.PhoneNumber, id, customerName, ProductNames);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Twilio Notification Failed:{ex.Message}");
                }
            }
            return View(id);
        }


        public IActionResult Plus(int id)
        {
            var cart = _unitofwork.shoppingCart.get(id);
            cart.count += 1;
            _unitofwork.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Minus(int id)
        {
            var cart = _unitofwork.shoppingCart.get(id);
            if (cart.count == 1)
                cart.count = 1;
            else
                cart.count -= 1;
            _unitofwork.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var cart = _unitofwork.shoppingCart.get(id);
            _unitofwork.shoppingCart.Remove(cart);
            _unitofwork.Save();
            return RedirectToAction("Index");
        }
    }

}

