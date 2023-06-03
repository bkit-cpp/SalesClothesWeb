using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EShop.ViewModels.Catalog.Categories
{
    public class CategoryViewModels
    {
        public int Id { set; get; }

        [Display(Name = "Tên")]
        public string Name { get; set; }
        [Display(Name = "ID sản phẩm cha")]
        public int? ParentId { set; get; }

        [Display(Name = "Thứ tự")]
        public int SortOrder { set; get; }

        [Display(Name = "Trạng thái hoạt động")]
        public Status Status { get; set; }

        [Display(Name = "Mô tả chi tiết")]
        public string SeoDesription { get; set; }

        [Display(Name = "Tiêu đề")]
        public string SeoTitle { get; set; }

    }
}