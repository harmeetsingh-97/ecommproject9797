using ecomm_project.DataAccess.Repository;
using ecomm_project.DataAccess.Repository.Irepository;
using ecomm_project.models;
using ecomm_utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecomm_project.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = SD.role_Admin + "," + SD.role_Employee)]

    public class CoverTypeController : Controller
    {
        private readonly iunitofwork _unitofwork;
        public CoverTypeController(iunitofwork unitofwork)
        {
            _unitofwork=unitofwork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult upsert(int? id)
        {
            covertype covertype = new covertype();
            if (id == null) return View(covertype);
            covertype = _unitofwork.CoverType.get(id.GetValueOrDefault());
            if (covertype == null) return BadRequest();
            return View(covertype);
        }
        [HttpPost]
        public IActionResult upsert(covertype covertype)
        {
            if(covertype == null) return BadRequest();
            if(!ModelState.IsValid) return View(covertype);
            if(covertype.id == 0)_unitofwork.CoverType.Add(covertype);
            else
            {
                _unitofwork.CoverType.Update(covertype);
            }
            _unitofwork.Save();
            return RedirectToAction("Index");
        }
        #region API's
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitofwork.CoverType.GetAllOrders() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var covertypeindb = _unitofwork.CoverType.get(id);
            if (covertypeindb == null) return Json(new { Success = false, Message = "unable to delete data" });
            _unitofwork.CoverType.Remove(covertypeindb);
            _unitofwork.Save();
            return Json(new { Success = true, Message = "Data Deleted Successfully" });
        }
        #endregion
    }

}

