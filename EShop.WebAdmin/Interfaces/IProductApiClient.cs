using EShop.ViewModels.Catalog.Products;
using EShop.ViewModels.Common;
using System.Threading.Tasks;

namespace EShop.WebAdmin.Interfaces
{
    public interface IProductApiClient
    {
        Task<ApiResult<PagedResult<ProductVm>>> GetPagings(GetManageProductPagingRequest request);
        Task<bool> CreateProduct(ProductCreateRequest request);
        Task<bool> DeleteProduct(int Id);
        Task<bool> UpdateProduct(ProductUpdateRequest request);
    }
}