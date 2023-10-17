using EShop.ViewModels.Systems.Slides;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Interfaces
{
    public interface ISlideService
    {
        Task<List<SlideVm>> GetAll();
    }
}
