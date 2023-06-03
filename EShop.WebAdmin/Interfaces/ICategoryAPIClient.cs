using EShop.ViewModels.Catalog.Categories;
using EShop.ViewModels.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EShop.WebAdmin.Interfaces
{
    public interface ICategoryAPIClient
    {
        Task<ApiResult<CategoryViewModels>> GetById(string languageId, int id);

        Task<ApiResult<List<CategoryViewModels>>> GetListCatagory(string languageId);

        Task<ApiResult<bool>> CreateCatagory (CategoryCreateRequest request);

        Task<ApiResult<bool>> UpdateStatus(int id);
    }
}
