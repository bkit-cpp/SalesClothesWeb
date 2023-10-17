using EShop.ApiIntegration;
using EShop.ViewModels.Catalog.Products;
using EShop.ViewModels.Common;
using EShop.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryAPIClient _categoryAPIClient;
        public ProductController(IProductApiClient productApiClient, ICategoryAPIClient categoryAPIClient)
        {
            _productApiClient = productApiClient;
            _categoryAPIClient = categoryAPIClient;
        }
        public async Task< IActionResult> Detail(int id, string culture)
        {
            var products = await _productApiClient.GetById(id, culture);
            return View(new ProductDetailViewModel()
            {
                Product = products
            }) ;
        }
        public  async Task<IActionResult> Category(int id, string culture)
        {
            var category = await _categoryAPIClient.GetById(culture, id);
            return View(new ProductCategoryViewModel()
            {
                Category = category
            }) ;

        }
    }
}
