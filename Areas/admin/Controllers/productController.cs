using ecomm_project.DataAccess.Repository.Irepository;
using ecomm_project.models;
using ecomm_project.models.ViewModels;
using ecomm_utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace ecomm_project.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = SD.role_Admin + "," + SD.role_Employee)]

    public class productController : Controller
    {
        private readonly iunitofwork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public productController(iunitofwork unitofwork,
            IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitofwork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new product(),
                CategoryList = _unitofwork.category.GetAllOrders().Select(cl => new SelectListItem()
                {
                    Text = cl.name,
                    Value = cl.id.ToString()
                }),
                CoverTypeList = _unitofwork.CoverType.GetAllOrders().Select(ct => new SelectListItem()
                {
                    Text = ct.name,
                    Value = ct.id.ToString()
                })
            };
            if (id == null) return View(productVM);
            productVM.Product = _unitofwork.product.get(id.GetValueOrDefault());
            if (productVM.Product == null) return NotFound();
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM)
                {
            if (ModelState.IsValid)
            {
                var WebRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                { 
                    var filename = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(files[0].FileName);
                    var uploads = Path.Combine(WebRootPath, "images\\products");
                    if (productVM.Product.id != 0)
                    {
                        var imageExists = _unitofwork.product.get(productVM.Product.id).ImageUrl;
                        productVM.Product.ImageUrl = imageExists;
                    }
                    if (productVM.Product.ImageUrl != null)
                    {
                        var imagePath = Path.Combine(WebRootPath, productVM.Product.ImageUrl.Trim('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var FileStream = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(FileStream);
                      
                    }
                      productVM.Product.ImageUrl = @"\images\products\" + filename + extension;

                }
                else
                {
                    var imageExists = _unitofwork.product.get(productVM.Product.id).ImageUrl;
                    productVM.Product.ImageUrl = imageExists;                   
                }
                if (productVM.Product.id == 0)
                    _unitofwork.product.Add(productVM.Product);
                else
                    _unitofwork.product.Update(productVM.Product);
                _unitofwork.Save();
                return RedirectToAction(nameof(Index));

            }
            else
            {
                ProductVM ProductVM = new ProductVM()
                {
                    Product = new product(),
                    CategoryList = _unitofwork.category.GetAllOrders().Select(cl => new SelectListItem()
                    {
                        Text = cl.name,
                        Value = cl.id.ToString()
                    }),
                    CoverTypeList = _unitofwork.CoverType.GetAllOrders().Select(ct => new SelectListItem()
                    {
                        Text = ct.name,
                        Value = ct.id.ToString()
                    })
                };
                if (productVM.Product.id != 0)
                {
                    productVM.Product = _unitofwork.product.get(productVM.Product.id);
                    if (productVM.Product == null) return NotFound();
                }
                return View(productVM);
            }
            

        }
    



        #region API's
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitofwork.product.GetAllOrders() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productindb = _unitofwork.product.get(id);
            if (productindb == null) return Json(new { success = false, Message = "Unable To Delete Data" });
            var WebRootPath = _webHostEnvironment.WebRootPath;
            var imagePath = Path.Combine(WebRootPath, productindb.ImageUrl.Trim('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _unitofwork.product.Remove(productindb);
            _unitofwork.Save();
            return Json(new { success = true, Message = "Data Delete Successfully" });

        }
        #endregion
    }
}
