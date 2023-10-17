using EShop.Application.Interfaces;
using EShop.Data.EF;
using EShop.ViewModels.Systems.Slides;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Services.Utilities
{
    public class SlideService : ISlideService
    {
        private readonly EShopDbContext _context;
        public SlideService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<List<SlideVm>> GetAll()
        {
            var slides = await _context.Slides.OrderBy(x => x.SortOrder)
                .Select(x => new SlideVm()
                {
                    Id=x.Id,
                    Description=x.Description,
                    Name=x.Name,
                    Url=x.Url,
                    Image=x.Image
                }).ToListAsync();
            return slides;
        }
    }
}
