using ecomm_project.DataAccess.Repository.Irepository;
using ecomm_project.models;
using ecomm_utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecomm_project.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = SD.role_Admin)]

    public class CompanyController : Controller
    {
        private readonly iunitofwork _unitofwork;
        public CompanyController(iunitofwork unitofwork)
        {
            _unitofwork= unitofwork;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region API's
        public IActionResult GetAll()
        {
            return Json( new {data=_unitofwork.company.GetAllOrders()});
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var companyindb = _unitofwork.company.get(id);
            if (companyindb == null) return Json(new { success = false, message = "Unable to Delete Data!!!" });
            _unitofwork.company.Remove(companyindb);
            _unitofwork.Save();
            return Json(new { success = true, message = "Data Deleted Successfully" });
        }
        #endregion
        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id == null) return View(company);
            company = _unitofwork.company.get(id.GetValueOrDefault());
            if (company == null) return NotFound();
            return View(company);
        }
        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (company == null) return View(company);
            if (!ModelState.IsValid) return View(company);
            if (company.id == 0) _unitofwork.company.Add(company);
            else
                _unitofwork.company.Update(company);
            _unitofwork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}

