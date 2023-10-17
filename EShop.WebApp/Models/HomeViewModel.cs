using EShop.ViewModels.Catalog.Products;
using EShop.ViewModels.Systems.Slides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.WebApp.Models
{
    public class HomeViewModel
    {
        public List<SlideVm> Slides { get; set; }
        public List<ProductVm> FeaturedProducts { get; set; }
        public List<ProductVm> LatestProducts { get; set; }
    }
}
