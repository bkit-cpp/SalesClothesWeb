using EShop.ViewModels.Catalog.Categories;
using EShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModels>> GetAllAsync(string languageId);

        Task<ApiResult<int>> CreateAsync(CategoryCreateRequest request);

        Task<ApiResult<CategoryViewModels>> GetById(string languageId, int id);

        Task<ApiResult<bool>> Update(int id);
    }
}