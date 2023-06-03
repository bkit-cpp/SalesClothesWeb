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
        public Task CreateAsync(ProductInCategory productInCategory)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategoryId(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductId(int id)
        {
            throw new NotImplementedException();
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
