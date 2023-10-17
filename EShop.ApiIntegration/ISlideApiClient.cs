using EShop.ViewModels.Systems.Slides;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ApiIntegration
{
   public interface ISlideApiClient
    {
        Task<List<SlideVm>>GetAll();
    }
}
