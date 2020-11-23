using ElevenNote.Models;
using ElevenNote.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNoteMVC.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            var service = new CategoryService();
            var model = service.GetCategories();
            return View(model);
        }

        //GET: Create Category
        public ActionResult Create()
        {
            return View();
        }
        //POST: Create Category
        [HttpPost]
        public ActionResult Create(CategoryCreate model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var service = new CategoryService();
            if (!service.CreateCategory(model))
            {
                ModelState.AddModelError("", "Your category could not be created");
                return View(model);
            }
            TempData["SaveResult"] = "Category created successfully!";
            return RedirectToAction("Index");
        }
    }
}