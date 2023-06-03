using EShop.Data.Entities;
using EShop.ViewModels.Common;
using EShop.WebAdmin.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EShop.Utilities.Constants;
using EShop.ViewModels.Catalog.Products;

namespace EShop.WebAdmin.Service
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string HMACSHA256(string text, string key)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] textBytes = encoding.GetBytes(text);
            byte[] keyBytes = encoding.GetBytes(key);
            byte[] hashBytes;
            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public async Task<ApiResult<PagedResult<ProductVm>>> GetPagings(GetManageProductPagingRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ServerHostIP"]);

                var secretKey = _configuration["SecretKey"];
                var path = $"/api/product/all?pageIndex={request.PageIndex}" + $"&pageSize={request.PageSize}" + $"&keyword={request.Keyword}&languageId={request.LanguageId}";
                var generateToken = HMACSHA256(path, secretKey);
                client.DefaultRequestHeaders.Add("Token", generateToken);

                var response = await client.GetAsync(path);
                var body = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiSuccessResult<PagedResult<ProductVm>>>(body);
                return data;
            }
            catch (Exception e)
            {
                throw new BadRequestException(e.ToString(), 401);
            }
        }
        public async Task<bool> CreateProduct(ProductCreateRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ServerHostIP"]);
               
                var json = JsonConvert.SerializeObject(request);
                var path = "/api/product/";
                var secretKey = _configuration["SecretKey"];
                var generateToken = HMACSHA256(path, secretKey);
                var requestContent = new MultipartFormDataContent();
              // var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
                if (request.ThumbnailImage != null)
                {
                    byte[] data;
                    using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
                }

                requestContent.Add(new StringContent(request.Price.ToString()), "price");
                requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalPrice");
                requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Details) ? "" : request.Details.ToString()), "details");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoDescription) ? "" : request.SeoDescription.ToString()), "seoDescription");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoTitle) ? "" : request.SeoTitle.ToString()), "seoTitle");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoAlias) ? "" : request.SeoAlias.ToString()), "seoAlias");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.LanguageId) ? "" : request.LanguageId.ToString()), "languageId");
              // requestContent.Add(new StringContent(languageId), "languageId");
                var response = await client.PostAsync(path, requestContent);
                var result = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }
        }

        public async Task<bool> UpdateProduct(ProductUpdateRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ServerHostIP"]);
                
                var json = JsonConvert.SerializeObject(request);
                var path = "/api/product/";
                var secretKey = _configuration["SecretKey"];
                var generateToken = HMACSHA256(path, secretKey);
                
                var requestContent = new MultipartFormDataContent();
                if (request.ThumbnailImage != null)
                {
                    byte[] data;
                    using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
                }
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Details) ? "" : request.Details.ToString()), "details");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoDescription) ? "" : request.SeoDescription.ToString()), "seoDescription");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoTitle) ? "" : request.SeoTitle.ToString()), "seoTitle");
                requestContent.Add(new StringContent(string.IsNullOrEmpty(request.SeoAlias) ? "" : request.SeoAlias.ToString()), "seoAlias");

                var response = await client.PutAsync(path + request.Id, requestContent);

                var result = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }

        }

        public async Task<bool> DeleteProduct(int Id)
        {
            throw new NotImplementedException();
        }
    }
}