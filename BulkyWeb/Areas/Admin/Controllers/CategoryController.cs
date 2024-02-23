using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        // Remember ApplicationDbContext comes from the dependency injection added to the container in the Program.cs file.
        //private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;

        // Before UnitOfWork added. You can see the advantages of using UnitOfWork Here. Cons are that if you aren't using a particular interface you still
        // have to create it, but UnitOfWork is much cleaner.
        //public CategoryController(ICategoryRepository db)
        //{
        //    _categoryRepo = db;
        //}
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //var objCategoryList = _categoryRepo.GetAll().ToList();
            var objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                // "Name" in AddModelError is the control that the error will appear next to.
                ModelState.AddModelError("Name", "The Display Order cannot match the Name.");
            }
            //if (obj.Name != null && obj.Name.ToLower() == "test")
            //{
            //    // "" in AddModelError will only display in Summary
            //    ModelState.AddModelError("", "Test is an invalid value.");
            //}
            if (ModelState.IsValid)
            {
                //_categoryRepo.Add(obj);
                //_categoryRepo.Save();
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();

                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var category = _categoryRepo.Get(u=>u.Id == id);
            var category = _unitOfWork.Category.Get(u => u.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                //_categoryRepo.Update(obj);
                //_categoryRepo.Save();
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();

                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var category = _categoryRepo.Get(u => u.Id == id);
            var category = _unitOfWork.Category.Get(u => u.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            //var obj = _categoryRepo.Get(u => u.Id == id);
            var obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            //_categoryRepo.Remove(obj);
            //_categoryRepo.Save();
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();

            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }

    }
}
