using EShop.ApiIntegration;
using EShop.Utilities.Constants;
using EShop.WebApp.Models;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISlideApiClient _slideApiClient;
        private readonly IProductApiClient _productApiClient;
        private readonly ISharedCultureLocalizer _loc;
        public HomeController(ILogger<HomeController> logger, ISlideApiClient slideApiClient,IProductApiClient productApiClient, ISharedCultureLocalizer loc)
        {
            _logger = logger;
            _slideApiClient = slideApiClient;
            _productApiClient = productApiClient;
            _loc = loc;
        }

        public async Task< IActionResult> Index()
        {
            var culture = CultureInfo.CurrentCulture.Name;
            var user = User.Identity.Name;
            var viewmodel = new HomeViewModel
            {
                Slides = await _slideApiClient.GetAll(),
                FeaturedProducts = await _productApiClient.GetFeaturedProducts(culture, SystemConstants.ProductsSettings.NumberOfFeaturedProducts),
                LatestProducts=await _productApiClient.GetLatestProducts(culture, SystemConstants.ProductsSettings.NumberOfLatestProducts)
            };
            return View(viewmodel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult SetCultureCookie(string cltr,string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            return new LocalRedirectResult(returnUrl);
        }


    }
}
