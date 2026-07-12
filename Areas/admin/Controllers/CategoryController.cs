using ecomm_project.DataAccess.Repository.Irepository;
using ecomm_project.models;
using ecomm_utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace ecomm_project.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles=SD.role_Admin+","+SD.role_Employee)]
    
    public class CategoryController : Controller
    {
        private readonly iunitofwork _unitofwork;
        public CategoryController(iunitofwork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult upsert(int? id)
        {
            //create
            category category = new category();
            if (id == null) return View(category);
            //edit
            category = _unitofwork.category.get(id.GetValueOrDefault());
                if (category == null) return NotFound();
            return View(category);
                

        }
        [HttpPost]
        public IActionResult upsert(category category)
        {
            if (category == null) return BadRequest();
            if (!ModelState.IsValid) return View(category);
            if (category.id == 0)
                _unitofwork.category.Add(category);
            else
            {
                _unitofwork.category.Update(category);
            }
            _unitofwork.Save();
            return RedirectToAction(nameof(Index));
            
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var categorylist = _unitofwork.category.GetAllOrders();
            return Json(new { data =categorylist });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var categoryindb = _unitofwork.category.get(id);
            if (categoryindb == null) return Json(new { success = false, Message = "Unable To Delete Data" });
            _unitofwork.category.Remove(categoryindb);
            _unitofwork.Save();
            return Json(new { success = true, Message = "Data Delete Successfully" });
            
        }
        #endregion
    }
}

