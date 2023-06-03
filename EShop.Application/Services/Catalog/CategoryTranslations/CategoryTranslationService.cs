using EShop.Data.EF;
using EShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Catalog.CategoryTranslations
{
    public class CategoryTranslationService : ICategoryTranslationService
    {
        private EShopDbContext _context;

        public CategoryTranslationService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CategoryTranslation CategoryTranslations)
        {
            _context.CategoryTranslations.Add(CategoryTranslations);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var categorytranslation = GetById(id);
            _context.CategoryTranslations.Remove(categorytranslation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryID(int id)
        {
            var categorytranslation = GetByCategoryID(id);
            _context.CategoryTranslations.Remove(categorytranslation);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<CategoryTranslation> GetAllAsync()
        {
            return _context.CategoryTranslations.ToList();
        }

        public CategoryTranslation GetByCategoryID(int id)
        {
            return _context.CategoryTranslations.Where(x => x.CategoryId == id).FirstOrDefault();
        }

        public CategoryTranslation GetById(int id)
        {
            return _context.CategoryTranslations.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task UpdateAsync(CategoryTranslation CategoryTranslations)
        {
            _context.CategoryTranslations.Update(CategoryTranslations);
            await _context.SaveChangesAsync();
        }
    }
}