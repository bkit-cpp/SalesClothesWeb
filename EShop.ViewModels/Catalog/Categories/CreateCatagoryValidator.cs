using EShop.ViewModels.Catalog.Categories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.ViewModel.Catalog.Categories
{
    public class CreateCatagoryValidator : AbstractValidator<CategoryCreateRequest>
    {
        public CreateCatagoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
             .MaximumLength(200).WithMessage("Name can not over 200 characters");

            RuleFor(x => x.SeoAlias).NotEmpty().WithMessage("SeoAlias is required")
                .MaximumLength(100).WithMessage("SeoAlias can not over 200 characters"); 
            
            RuleFor(x => x.SeoDescription).NotEmpty().WithMessage("SeoDescription is required")
                .MaximumLength(100).WithMessage("SeoDescription can not over 200 characters");            
            
            RuleFor(x => x.SeoTitle).NotEmpty().WithMessage("SeoTitle is required")
                .MaximumLength(100).WithMessage("SeoTitle can not over 200 characters");

            RuleFor(x => x.SortOrder).NotEmpty().WithMessage("SortOrder is required");
        }
    }
}
