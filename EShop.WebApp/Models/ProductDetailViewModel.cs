using EShop.ViewModels.Catalog.Categories;
using EShop.ViewModels.Catalog.ProductImages;
using EShop.ViewModels.Catalog.Products;
using EShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.WebApp.Models
{
    public class ProductDetailViewModel
    {
        public CategoryViewModels Category { get; set; }
        public ApiResult<ProductVm> Product { get; set; }
        public List<ProductVm> RelatedProducts { get; set; }
        public List<ProductImageViewModel> ProductImages { get; set; }
    }
}
