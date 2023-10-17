using EShop.Data.Entities;
using EShop.ViewModels.Catalog.Categories;
using EShop.ViewModels.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ApiIntegration
{
    public class CategoryApiClient : ICategoryAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
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

        public async Task <bool> CreateCatagory(CategoryCreateRequest request)
        {
            try
            {

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ServerHostIP"]);
                
                var json = JsonConvert.SerializeObject(request);
                var httpContext = new StringContent(json, Encoding.UTF8, "application/json");
                var path = "/api/Category";
                var secretKey = _configuration["SecretKey"];
                var generateToken = HMACSHA256(path, secretKey);
                httpContext.Headers.Add("Token", generateToken);
                var response = await client.PostAsync(path, httpContext);
                var result = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode;
                    
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
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ServerHostIP"]);
                var secretKey = _configuration["SecretKey"];
                var path = $"/api/Category/{languageId}/{id}";
                var generateToken = HMACSHA256(path, secretKey);
                client.DefaultRequestHeaders.Add("Token", generateToken);
                var response = await client.GetAsync(path);
                var body = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject
                      <CategoryViewModels>(body);
                return data;
            }
            catch(Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 404);
            }
        }

        public async Task<List<CategoryViewModels>> GetListCatagory(string languageId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ServerHostIP"]);

                var secretKey = _configuration["SecretKey"];
                var path = "/api/Category?languageId=" + $"{languageId}";
                var generateToken = HMACSHA256(path, secretKey);
                client.DefaultRequestHeaders.Add("Token", generateToken);

                var response = await client.GetAsync(path);
                var body = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject
                 <List<CategoryViewModels>>(body);
                return data;
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }
        }

        public Task<ApiResult<bool>> UpdateStatus(int id)
        {
            throw new NotImplementedException();
        }
    }
}
