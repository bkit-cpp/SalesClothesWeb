using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.ViewModels.Common
{
    public class PagedResultBase
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }


    }
}