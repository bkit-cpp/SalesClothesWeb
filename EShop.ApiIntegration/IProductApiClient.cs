using EShop.ViewModels.Catalog.Products;
using EShop.ViewModels.Common;
using EShop.ViewModels.Common.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EShop.ApiIntegration
{
    public interface IProductApiClient
    {
        Task<ApiResult<PagedResult<ProductVm>>> GetPagings(GetManageProductPagingRequest request);
        Task<bool> CreateProduct(ProductCreateRequest request);
        Task<ApiResult<ProductVm>> GetById(int id, string languageId);
        Task<ApiResult<bool>> DeleteProduct(int productId);
        Task<bool> UpdateProduct(ProductUpdateRequest request);
        Task<List<ProductVm>> GetFeaturedProducts(string languageId, int take);
        Task<List<ProductVm>> GetLatestProducts(string languageId, int take);
    }
}