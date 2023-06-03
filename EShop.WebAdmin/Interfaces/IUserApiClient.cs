using EShop.Data.Entities;
using EShop.ViewModels.Common;
using EShop.ViewModels.System.Users;
using EShop.ViewModels.Systems.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.WebAdmin.Interfaces
{
    public interface IUserApiClient
    {
        Task<ApiResult<User>> Login(LoginRequest request);

        Task<ApiResult<PagedResult<UserVm>>> GetListUserPagging(GetListUserPagingRequest request);

        Task<ApiResult<bool>> RegisterUser(RegisterRequest request);

        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request);

        Task<ApiResult<bool>> Delete(Guid id);

        Task<ApiResult<UserVm>> GetById(Guid id);
    } 
}
