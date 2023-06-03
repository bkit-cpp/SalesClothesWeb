using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Application.Interfaces
{
    public interface IEShopAuthentication
    {
        void Authentication(HttpRequest request);
    }
}
