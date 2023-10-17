using EShop.ApiIntegration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.WebApp.Controllers.Components
{
    public class SideBarViewComponent: ViewComponent
    {
        private readonly ICategoryAPIClient _categoryAPIClient;
        public SideBarViewComponent(ICategoryAPIClient categoryAPIClient)
        {
            _categoryAPIClient = categoryAPIClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _categoryAPIClient.GetListCatagory(CultureInfo.CurrentCulture.Name);
            return View(items);
        }
    }
}
