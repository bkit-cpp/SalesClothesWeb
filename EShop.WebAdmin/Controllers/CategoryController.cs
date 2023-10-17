
using EShop.ApiIntegration;
using EShop.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.WebAdmin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryAPIClient _categoryAPIClient;
        public CategoryController(ICategoryAPIClient categoryAPIClient)
        {
            _categoryAPIClient = categoryAPIClient;
        }
     
        public async Task<IActionResult> ViewListCategory()
        {

            var data = await _categoryAPIClient.GetListCatagory("vi");
            //ViewBag support view MVC

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }

            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _categoryAPIClient.CreateCatagory(request);
            if (result)
            {
                TempData["result"] = "Thêm mới Danh Muc thành công";
                return RedirectToAction("ViewListCategory");
            }

            ModelState.AddModelError("", "Thêm Danh Muc thất bại");
            return View(request);
        }

    }
}
