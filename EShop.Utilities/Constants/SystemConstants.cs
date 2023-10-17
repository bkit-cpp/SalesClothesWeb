using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Utilities.Constants
{
    public class SystemConstants
    {
        public const string MainConnectionString = "eShopSolutionDb";
        public const string CartSession = "CartSession";
        public class AppSettings
        {
            public const string DefaultLanguageId = "vi";
            public const string Token = "Token";
            public const string BaseAddress = "BaseAddress";
        }
        public class ProductsSettings
        {
            public const int NumberOfFeaturedProducts = 4;
            public const int NumberOfLatestProducts = 6;
        }
        public class ProductConstants
        {
            public const string NA = "N/A";
        }
    }
}