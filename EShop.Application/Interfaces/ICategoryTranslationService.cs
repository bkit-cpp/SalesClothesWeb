
using EShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Catalog.CategoryTranslations
{
    public interface ICategoryTranslationService
    {
        Task CreateAsync(CategoryTranslation CategoryTranslations);

        Task UpdateAsync(CategoryTranslation CategoryTranslations);

        Task DeleteAsync(int id);

        Task DeleteCategoryID(int id);

        IEnumerable<CategoryTranslation> GetAllAsync();

        CategoryTranslation GetById(int id);

        CategoryTranslation GetByCategoryID(int id);
    }
}