using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Data.Entities
{
    public class Exceptions
    {
    }
    public class BadRequestException : Exception
    {
        public int StatusCode;
        public BadRequestException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
