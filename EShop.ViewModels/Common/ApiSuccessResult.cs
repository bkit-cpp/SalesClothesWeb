using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.ViewModels.Common
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(T resultObj)
        {
            IsSuccessed = true;
            Message = "Thành công";
            ResultObj = resultObj;
        }

        public ApiSuccessResult( )
        {
            IsSuccessed = true;
            Message = "Thành công";
            
        }
    }
}