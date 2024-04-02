using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {      
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {             
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            string roleId = _db.UserRoles.FirstOrDefault(x => x.UserId == userId).RoleId;

            RoleManagementVM roleVm = new RoleManagementVM()
            {
                ApplicationUser = _db.ApplicationUsers.Include(x=>x.Company).FirstOrDefault(x=>x.Id == userId),
                RoleList = _db.Roles.Select(x=> new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                }),
                CompanyList = _db.Companies.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            roleVm.ApplicationUser.Role = _db.Roles.FirstOrDefault(x => x.Id == roleId).Name;

            return View(roleVm);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
        {
            string roleId = _db.UserRoles.FirstOrDefault(x => x.UserId == roleManagementVM.ApplicationUser.Id).RoleId;
            string oldRole = _db.Roles.FirstOrDefault(x => x.Id == roleId)?.Name;
            if (roleManagementVM.ApplicationUser.Role != oldRole)
            {
                // A role was updated
                ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(x => x.Id == roleManagementVM.ApplicationUser.Id);
                if (roleManagementVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                }
                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _db.SaveChanges();

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();

            }   
            return RedirectToAction("Index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll() 
        { 
            List<ApplicationUser>userList = _db.ApplicationUsers.Include(x=>x.Company).ToList();
            
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in userList)
            {
                var roleId = userRoles.FirstOrDefault(x => x.UserId == user.Id)?.RoleId;
                user.Role = roles.FirstOrDefault(x => x.Id == roleId)?.Name;

               if (user.Company == null)
                {
                    user.Company = new Company() { Name = ""};   
                }
            }

            return Json(new {data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (objFromDb == null) 
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                // user is currently locked and we need to unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                // Lock them out
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message="Operation Successful"});
        }

        #endregion


    }
}
