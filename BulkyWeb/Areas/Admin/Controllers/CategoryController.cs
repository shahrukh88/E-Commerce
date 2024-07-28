
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
   [Authorize(Roles =SD.Role_Admin)]

    public class CategoryController : Controller
    {
       

        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
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
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the name");
            }

            if (ModelState.IsValid)
            {

                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created Successfully";
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

            Category? CategoryFromdb = _unitOfWork.Category.Get(u => u.Id == id);

            //  Category? CategoryFromdb1 = _db.Categories.FirstOrDefault(c => c.Id == id);
            // Category? CategoryFromdb2 = _db.Categories.Where(c => c.Id == id).FirstOrDefault();


            if (CategoryFromdb == null)
            {
                return NotFound();
            }
            return View(CategoryFromdb);
        }


        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {

                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";
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

            Category? CategoryFromdb = _unitOfWork.Category.Get(u => u.Id == id);

            if (CategoryFromdb == null)
            {
                return NotFound();
            }
            return View(CategoryFromdb);
        }


        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePOST(int? id)
        {
            Category? Obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (Obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(Obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted Successfully";
            return RedirectToAction("Index");

        }

    }
}
