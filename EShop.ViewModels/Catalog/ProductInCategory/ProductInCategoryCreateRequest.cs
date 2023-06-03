using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.ViewModels.Catalog.ProductInCategory
{
    public class ProductInCategoryCreateRequest
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }
}