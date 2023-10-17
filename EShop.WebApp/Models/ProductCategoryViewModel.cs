using EShop.ViewModels.Catalog.Categories;
using EShop.ViewModels.Catalog.Products;
using EShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.WebApp.Models
{
    public class ProductCategoryViewModel
    {
        public CategoryViewModels Category { get; set; }
        public PagedResult<ProductVm> Products { get; set; }
    }
}
