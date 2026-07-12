    using ecomm_project.DataAccess.Data;
using ecomm_project.DataAccess.Repository.Irepository;
using ecomm_project.models;
using ecomm_utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace ecomm_project.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = SD.role_Admin)]

    public class UserController : Controller
    {
        private readonly iunitofwork _unitofwork;
        private readonly ApplicationDbContext _context;
        public UserController(iunitofwork unitofwork, ApplicationDbContext context)
        {
            _unitofwork = unitofwork;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region API's
        [HttpGet]
        public IActionResult GetAll()
        {
            var userlist = _context.applicationUsers.ToList();
            var rolelist = _context.Roles.ToList();
            var userroles = _context.UserRoles.ToList();
            foreach (var user in userlist)
            {
                var roleid = userroles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.role = rolelist.FirstOrDefault(r => r.Id == roleid).Name;

                if (user.companyid == null)
                {
                    user.company = new Company()
                    {
                        name = ""
                    };
                }
                if (user.companyid != null)
                {
                    user.company = new Company()
                    {
                        name = _unitofwork.company.get(Convert.ToInt32(user.companyid)).name
                    };
                }

            }
            //remove admin  user
            var adminUser = userlist.FirstOrDefault(u => u.role == SD.role_Admin);
            userlist.Remove(adminUser);
            return Json(new { data = userlist });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            bool isLocked = false;
            var userindb = _unitofwork.applicationUser.FirstOrDefault(u => u.Id == id);
            if (userindb == null)
            {
                return Json(new { success = false, message = "Something Went Wrong While LockUnlock User" });
            }
            if(userindb !=null && userindb.LockoutEnd>DateTime.Now)
            {
                userindb.LockoutEnd = DateTime.Now;
                isLocked = false;
            }
            else
            {
                userindb.LockoutEnd = DateTime.Now.AddYears(100);
                isLocked = true;
            }
            _context.SaveChanges();
            return Json(new { success = true, message =   isLocked==true ? "User Successfully Locked" : "User Successfully UnLocked" });
        }
        
        #endregion

        
    }
}
