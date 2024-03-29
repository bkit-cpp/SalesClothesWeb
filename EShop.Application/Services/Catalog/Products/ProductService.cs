﻿using EShop.Application.Common;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EShop.Application.Interfaces;
using EShop.Data.EF;
using EShop.ViewModels.Common;
using EShop.Application.Services.Catalog.Products;
using EShop.Data.Entities;
using EShop.Utilities.Exceptions;
using EShop.ViewModels.Catalog.ProductImages;
using EShop.ViewModels.Catalog.Products;
using EShop.Utilities.Constants;

namespace EShop.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public ProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task AddViewcount(int productId)
        {
            var product = await _context.Product.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                IsFeatured = true,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name=request.Name,
                        Description=request.Description,
                        Details=request.Details,
                        SeoDescription=request.SeoDescription,
                        SeoAlias=request.SeoAlias,
                        SeoTitle=request.SeoTitle,
                        LanguageId=request.LanguageId
                    }
                }
            };
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product == null) throw new Exception($"Cannot find a project:{productId}");
            _context.Product.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<ApiResult<PagedResult<ProductVm>>> GetAllPaging(GetManageProductPagingRequest request)
        {
            try
            {
                //1. Select join
                var query = from p in _context.Product
                            join pt in _context.ProductTranslations on p.Id equals pt.ProductId 
                             join pic in _context.ProductImages.Where(x => x.IsDefault == true) on p.Id equals pic.ProductId
                            //join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                            //from pic in ppic.DefaultIfEmpty()
                            //join c in _context.Categories on pic.CategoryId equals c.Id into picc
                            //from c in picc.DefaultIfEmpty()
                            //where pt.LanguageId == request.LanguageId
                         
                            select new { p, pt, pic };
                //2.filter

                if (!string.IsNullOrEmpty(request.Keyword))
                    query = query.Where(x => x.pt.Name.Contains(request.Keyword));


                //if (request.CategoryId != null && request.CategoryId != 0)
                //{
                //    query = query.Where(p => p.pic.CategoryId == request.CategoryId);
                //}

                //3. Paging
                int totalRow = await query.CountAsync();

                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new ProductVm()
                    {
                        Id = x.p.Id,
                        Name = x.pt==null? SystemConstants.ProductConstants.NA:x.pt.Name,
                        DateCreated = x.p.DateCreated,
                        Description = x.pt == null ? SystemConstants.ProductConstants.NA : x.pt.Description,
                        Details = x.pt == null ? SystemConstants.ProductConstants.NA : x.pt.Details,
                        LanguageId = x.pt == null ? SystemConstants.ProductConstants.NA : x.pt.LanguageId,
                        OriginalPrice = x.p.OriginalPrice,
                        Price = x.p.Price,
                        SeoAlias = x.pt == null ? SystemConstants.ProductConstants.NA : x.pt.SeoAlias,
                        SeoDescription = x.pt.SeoDescription,
                        SeoTitle = x.pt == null ? SystemConstants.ProductConstants.NA : x.pt.SeoTitle,
                        Stock = x.p.Stock,
                        ViewCount = x.p.ViewCount,
                        ThumbnailImage = x.pic.ImagePath
                    }).ToListAsync();

                //4. Select and projection
                var pagedResult = new PagedResult<ProductVm>()
                {
                    TotalRecords = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = data
                };
                return new ApiSuccessResult<PagedResult<ProductVm>>(pagedResult);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }
        }

        public async Task<ApiResult<int>> Update(ProductUpdateRequest request)
        {
            try
            {
                var product = await _context.Product.FindAsync(request.Id);
                var productTranslations = await _context.ProductTranslations.
                    FirstOrDefaultAsync(x => x.ProductId == request.Id
                && x.LanguageId == request.LanguageId);

                if (product == null || productTranslations == null)
                    throw new EShopException
                   ($"Cannot find a product with id: {request.Id}");

                productTranslations.Name = request.Name;
                productTranslations.SeoAlias = request.SeoAlias;
                productTranslations.SeoDescription = request.SeoDescription;
                productTranslations.SeoTitle = request.SeoTitle;
                productTranslations.Description = request.Description;
                productTranslations.Details = request.Details;

                if (request.ThumbnailImage != null)
                {
                    var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync
                        (i => i.IsDefault == true
                    && i.ProductId == request.Id);
                    if (thumbnailImage != null)
                    {
                        thumbnailImage.FileSize = request.ThumbnailImage.Length;
                        thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                        _context.ProductImages.Update(thumbnailImage);
                    }
                }
                var result = await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(result);
            } catch (Exception ex)
            {
                throw new BadRequestException(ex.Message.ToString(), 401);
            }

        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product == null) throw new EShopException($"Cannot find a product with id: {productId}");
            product.Stock += addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<ApiResult<ProductVm>> GetById(int productId, string languageId)
        {
            try
            {
                var product = await _context.Product.FindAsync(productId);
                var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == languageId);
                var images = await _context.ProductImages.Where(x => x.ProductId == productId && x.IsDefault == true).FirstOrDefaultAsync();
                var productViewModel = new ProductVm()
                {
                    Id = product.Id,
                    DateCreated = product.DateCreated,
                    Description = productTranslation != null ? productTranslation.Description : null,
                    LanguageId = productTranslation.LanguageId,
                    Details = productTranslation != null ? productTranslation.Name : null,
                    Name = productTranslation != null ? productTranslation.Name : null,
                    OriginalPrice = product.OriginalPrice,
                    Price = product.Price,
                    SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                    SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                    SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                    Stock = product.Stock,
                    ViewCount = product.ViewCount,
                    ThumbnailImage=images!=null?images.ImagePath:"no images.jpg"
                };
                return new ApiSuccessResult<ProductVm>(productViewModel);
            } catch (Exception ex)
            {
                throw new BadRequestException(ex.Message.ToString(), 401);
            }

        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);

            if (productImage == null)
                throw new EShopException($"Cannot find a product with id: {imageId}");
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new EShopException($"Cannot find a product with id: {imageId}");
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
               .Select(i => new ProductImageViewModel()
               {
                   Caption = i.Caption,
                   DateCreated = i.DateCreated,
                   FileSize = i.FileSize,
                   Id = i.Id,
                   ImagePath = i.ImagePath,
                   IsDefault = i.IsDefault,
                   ProductId = i.ProductId,
                   SortOrder = i.SortOrder
               }).ToListAsync();
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new EShopException($"Cannot find a product with id: {imageId}");
            var viewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                DateCreated = image.DateCreated,
                FileSize = image.FileSize,
                Id = image.Id,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder
            };
            return viewModel;
        }
        public async Task<List<ProductVm>> GetAll(string languageId)
        {
            var query = from p in _context.Product
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };

            var data = await query
               .Select(x => new ProductVm()
               {
                   Id = x.p.Id,
                   Name = x.pt.Name,
                   DateCreated = x.p.DateCreated,
                   Description = x.pt.Description,
                   Details = x.pt.Details,
                   LanguageId = x.pt.LanguageId,
                   OriginalPrice = x.p.OriginalPrice,
                   Price = x.p.Price,
                   SeoAlias = x.pt.SeoAlias,
                   SeoDescription = x.pt.SeoDescription,
                   SeoTitle = x.pt.SeoTitle,
                   Stock = x.p.Stock,
                   ViewCount = x.p.ViewCount,

               }).ToListAsync();
            return data;
        }


        public async Task<PagedResult<ProductVm>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            var query = from p in _context.Product
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductVm()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,

                }).ToListAsync();


            var pagedResult = new PagedResult<ProductVm>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;

        }

        public async Task<List<ProductVm>> GetFeaturedProduct(string languageId, int take)
        {

            //1. Select join
            var query = from p in _context.Product
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductImages.Where(x => x.IsDefault == true) on p.Id equals pic.ProductId

                        //join pi in _context.ProductInCategories on p.Id equals pi.ProductId into ppic
                        //from pic in ppic.DefaultIfEmpty()
                        //join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        //from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == languageId
                        select new { p, pt,pic };
            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                    .Select(x => new ProductVm()
                    {
                        Id = x.p.Id,
                        Name = x.pt.Name,
                        DateCreated = x.p.DateCreated,
                        Description = x.pt.Description,
                        Details = x.pt.Details,
                        LanguageId = x.pt.LanguageId,
                        OriginalPrice = x.p.OriginalPrice,
                        Price = x.p.Price,
                        SeoAlias = x.pt.SeoAlias,
                        SeoDescription = x.pt.SeoDescription,
                        SeoTitle = x.pt.SeoTitle,
                        Stock = x.p.Stock,
                        ViewCount = x.p.ViewCount,
                        ThumbnailImage=x.pic.ImagePath
                    }).ToListAsync();
                return data;
        }

        public async Task<List<ProductVm>> GetLatestProduct(string languageId, int take)
        {
            var query = from p in _context.Product
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductImages.Where(x => x.IsDefault == true) on p.Id equals pic.ProductId
                        join ct in _context.ProductInCategories on p.Id equals ct.CategoryId 
                        //join pi in _context.ProductInCategories on p.Id equals pi.ProductId into ppic
                        //from pic in ppic.DefaultIfEmpty()
                        //join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        //from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == languageId && (pic == null || pic.IsDefault == true)
                        select new { p, pt, pic , ct};
            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                    .Select(x => new ProductVm()
                    {
                        Id = x.p.Id,
                        Name = x.pt.Name,
                        DateCreated = x.p.DateCreated,
                        Description = x.pt.Description,
                        Details = x.pt.Details,
                        LanguageId = x.pt.LanguageId,
                        OriginalPrice = x.p.OriginalPrice,
                        Price = x.p.Price,
                        SeoAlias = x.pt.SeoAlias,
                        SeoDescription = x.pt.SeoDescription,
                        SeoTitle = x.pt.SeoTitle,
                        Stock = x.p.Stock,
                        ViewCount = x.p.ViewCount,
                        ThumbnailImage = x.pic.ImagePath
                    }).ToListAsync();
            return data;
        }
    }
}
