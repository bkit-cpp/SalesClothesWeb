using EShop.Application.Interfaces;
using EShop.Data.EF;
using EShop.Data.Entities;
using EShop.ViewModels.Catalog.Categories;
using EShop.ViewModels.Common;
using EShop.Application.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Services.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private EShopDbContext _context;
        private readonly IStorageService _storageService;
        public CategoryService(EShopDbContext context,IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<ApiResult<int>> CreateAsync(CategoryCreateRequest request)
        {
            try
            {
                var category = new Category()
                {
                    IsShowOnHome=request.IsShowOnHome,
                    SortOrder=request.SortOrder,
                    ParentId=request.ParentId,
                    CategoryTranslations=new List<CategoryTranslation>()
                    {
                        new CategoryTranslation()
                        {
                            Name=request.Name,
                            SeoDescription=request.SeoDescription,
                            SeoAlias=request.SeoAlias,
                            SeoTitle=request.SeoTitle,
                            LanguageId=request.LanguageId
                        }
                    }
                };
                _context.Categories.Add(category);
                var result = await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(category.Id);

            }catch(Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }
        }

        public async Task<List<CategoryViewModels>> GetAllAsync(string languageId)
        {
            try
            {
              //  _context.Database.GetDbConnection().ToString();
                var query = from c in _context.Categories
                            join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                            where ct.LanguageId == languageId
                            select new { c, ct };
                  return await query.Select(x => new CategoryViewModels()
                {
                    Id = x.c.Id,
                    Name = x.ct.Name,
                    ParentId = x.c.ParentId,
                    SortOrder = x.c.SortOrder,
                    Status = (Status)x.c.Status,
                    SeoDesription = x.ct.SeoDescription,
                    SeoTitle = x.ct.SeoTitle
                }).ToListAsync();
            
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }
        }

        public async Task<CategoryViewModels> GetById(string languageId, int id)
        {
            try
            {
                var query = from c in _context.Categories
                            join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                            where ct.LanguageId == languageId && c.Id == id
                            select new { c, ct };
                var result = await query.Select(x => new CategoryViewModels()
                {
                    Id = x.c.Id,
                    Name = x.ct.Name,
                    ParentId = x.c.ParentId
                }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }
        }

        public async Task<ApiResult<bool>> Update(int id)
        {
            try
            {
                var cat = await _context.Categories.FindAsync(id);

                if (cat == null)
                {
                    return new ApiErrorResult<bool>($"Cannot find a catalog with id: {id}");
                }

                var s = cat.Status;

                if (s == 0)
                {
                    s = EShop.Data.Enums.Status.Active;
                }
                else
                {
                    s = EShop.Data.Enums.Status.InActive;
                }

                cat.Status = s;

                await _context.SaveChangesAsync();

                return new ApiSuccessResult<bool>();
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }
        }
    }
}
