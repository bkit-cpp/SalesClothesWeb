using EShop.ViewModels.Catalog.Categories;
using EShop.ViewModels.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EShop.ApiIntegration
{
    public interface ICategoryAPIClient
    {
        Task<CategoryViewModels> GetById(string languageId, int id);

        Task<List<CategoryViewModels>> GetListCatagory(string languageId);

        Task <bool> CreateCatagory (CategoryCreateRequest request);

        Task<ApiResult<bool>> UpdateStatus(int id);
    }
}
