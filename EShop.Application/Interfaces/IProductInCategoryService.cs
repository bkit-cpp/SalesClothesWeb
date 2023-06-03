
using EShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Interfaces
{
    public interface IProductInCategoryService
    {
        Task CreateAsync(ProductInCategory productInCategory);

        Task DeleteCategoryId(int id);

        Task DeleteProductId(int id);

        ProductInCategory GetByCategoryId(int id);

        ProductInCategory GetByProductId(int id);

        IEnumerable<ProductInCategory> GetAllAsync();
    }
}