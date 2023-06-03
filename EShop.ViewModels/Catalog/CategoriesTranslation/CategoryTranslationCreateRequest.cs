using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.ViewModels.Catalog.CategoriesTranslation
{
    public class CategoryTranslationCreateRequest
    {
        public int Id { set; get; }
        public int CategoryId { set; get; }
        public string Name { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }
        public string LanguageId { set; get; }
        public string SeoAlias { set; get; }
    }
}