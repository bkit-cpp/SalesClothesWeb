using EShop.Data.Entities;
using EShop.ViewModels.Common;
using EShop.ViewModels.System.Users;
using EShop.ViewModels.Systems.Users;
using System;
using System.Threading.Tasks;

namespace EShop.Application.Interfaces
{
    public interface IUserService
    {
         Task<ApiResult<User>> Login(LoginRequest request);

        Task<ApiResult<bool>> Register(RegisterRequest request);

        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);

        Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetListUserPagingRequest request);

        Task<ApiResult<UserVm>> GetById(Guid id);

        Task<ApiResult<bool>> Delete(Guid id);

    }
}