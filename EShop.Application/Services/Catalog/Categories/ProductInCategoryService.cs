using EShop.Application.Interfaces;
using EShop.Data.EF;
using EShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Services.Catalog.Categories
{
    public class ProductInCategoryService : IProductInCategoryService
    {
        private EShopDbContext _context;
        public ProductInCategoryService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(ProductInCategory productInCategory)
        {
            _context.ProductInCategories.Add(productInCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryId(int id)
        {
            var productincategory = GetByCategoryId(id);
            _context.ProductInCategories.Remove(productincategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductId(int id)
        {
            var productincategory = GetByCategoryId(id);
            _context.ProductInCategories.Remove(productincategory);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<ProductInCategory> GetAllAsync()
        {
            return _context.ProductInCategories.ToList();
        }

        public ProductInCategory GetByCategoryId(int id)
        {
            return _context.ProductInCategories.Where(x => x.CategoryId == id).FirstOrDefault();
        }

        public ProductInCategory GetByProductId(int id)
        {
            return _context.ProductInCategories.Where(x => x.ProductId == id).FirstOrDefault();
        }
    }
}
