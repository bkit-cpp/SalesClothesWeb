using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.BackendApi.Utils
{
    public class APIResult
    {
        public APIResult() { }

        public APIResult(int status, string message, string exception, object data)
        {
            Status = status;
            Message = message;
            Exception = exception;
            Data = data;
        }

        public int Status { set; get; }
        public string Message { set; get; }
        public string Exception { set; get; }
        public object Data { set; get; }

    }
}
