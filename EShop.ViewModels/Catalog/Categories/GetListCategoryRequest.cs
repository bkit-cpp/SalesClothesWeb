using EShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.ViewModel.Catalog.Categories
{
    public class GetListCategoryRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
