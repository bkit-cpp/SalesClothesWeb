using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EShop.Utilities.Constants;
using EShop.ViewModels.Catalog.Products;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using EShop.ApiIntegration;
using System;

namespace EShop.WebAdmin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoryAPIClient _categoryAPIClient;
        public ProductController(IProductApiClient productApiClient, IConfiguration configuration,ICategoryAPIClient categoryAPIClient)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
            _categoryAPIClient = categoryAPIClient;
        }
        public async Task<IActionResult> ViewListProducts(string keyword, int pageIndex = 1, int pageSize = 10)
        {
           
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = "vi",
               
                
            };
            var data = await _productApiClient.GetPagings(request);
            ViewBag.Keyword = keyword;
            string languagectId = "vi";
            var categories = await _categoryAPIClient.GetListCatagory(languagectId);
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }) ;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data.ResultObj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productApiClient.CreateProduct(request);
            if (result)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("ViewListProducts");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View(request);
        }
       
        [HttpGet]
        public async Task<IActionResult> Update(int id,string languageId)
        {
            var product =await _productApiClient.GetById(id, "vi");
            if (product.IsSuccessed)
            {
                var result = product.ResultObj;
                var productUpdate = new ProductUpdateRequest() 
                {
                    Id=result.Id,
                    Name=result.Name,
                    Description=result.Description,
                    Details=result.Details,
                    SeoDescription=result.SeoDescription,
                    SeoAlias=result.SeoAlias,
                    SeoTitle=result.SeoTitle,
                  //  ThumbnailImage= null,
                    LanguageId="vi",
                    

                };
                return View(productUpdate);
            }
            return RedirectToAction("Error", "Home");


        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update( ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productApiClient.UpdateProduct( request);
            if (result)
            {
                TempData["result"] = "Edit sản phẩm thành công";
                return RedirectToAction("ViewListProducts");
            }

            ModelState.AddModelError("", "Edit thất bại");
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
     
            var result = await _productApiClient.DeleteProduct(Id);
            if (result==null)
            {
                TempData["result"] = "Xóa thành công";
                return RedirectToAction("ViewListProducts");
            }
            
                return RedirectToAction("Error", "Home");
            
            
           // TempData["result"] = "Xóa thành công";
            

        }


    }
}