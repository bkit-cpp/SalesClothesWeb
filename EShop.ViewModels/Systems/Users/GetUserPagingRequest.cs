using EShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.ViewModels.Systems.Users
{
    public class GetListUserPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}