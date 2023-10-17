using EShop.Application.Interfaces;
using EShop.Data.EF;
using EShop.ViewModel.Systems.Languages;
using EShop.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Services.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IConfiguration _config;
        private readonly EShopDbContext _context;
        public LanguageService(EShopDbContext context, IConfiguration configuration)
        {
            _config = configuration;
            _context = context;
        }

        public async Task<ApiResult<List<LanguageVm>>> GetAll()
        {
            var languages = await _context.Languages
               .Select(x => new LanguageVm()
               {
                   Id = x.Id,
                   Name = x.Name,
                  
               }).ToListAsync();
            return new ApiSuccessResult<List<LanguageVm>>(languages);
        }
    }
}
