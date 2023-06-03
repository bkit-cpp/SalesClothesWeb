using EShop.ViewModels.Common;
using EShop.ViewModels.Systems.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.AdminApp.Services
{
    public interface IUserApiClient
    {
        Task<ApiErrorResult<string>> Login(LoginRequest request);
    }
}