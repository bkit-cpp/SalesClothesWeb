using EShop.ViewModels.Systems.Users;
using EShop.WebAdmin.Interfaces;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using EShop.ViewModels.Common;
using EShop.Data.Entities;
using Microsoft.Extensions.Configuration;
using EShop.WebAdmin.Utils;
using System.Security.Cryptography;
using EShop.ViewModels.System.Users;

//https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient

namespace EShop.WebAdmin.Service
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
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

        public async Task<ApiResult<PagedResult<UserVm>>> GetListUserPagging(GetListUserPagingRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ServerHostIP"]);

                var secretKey = _configuration["SecretKey"];
                var path = "/api/Users/Paging?PageIndex=" + $"{request.PageIndex}&PageSize={request.PageSize}&Keyword={request.Keyword}";
                var generateToken = HMACSHA256(path, secretKey);
                client.DefaultRequestHeaders.Add("Token", generateToken);

                var response = await client.GetAsync(path);
                var body = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApiSuccessResult<PagedResult<UserVm>>>(body);
                return data;
            } catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 400);
            }
        }

        public async Task<ApiResult<User>> Login(LoginRequest request)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var httpContext = new StringContent(json, Encoding.UTF8, "application/json");

                var path = "/api/Users/Login";
                var secretKey = _configuration["SecretKey"];
                var generateToken = HMACSHA256(path, secretKey);
                httpContext.Headers.Add("Token", generateToken);

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ServerHostIP"]);

                var response = await client.PostAsync(path, httpContext);

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ApiSuccessResult<User>>( await response.Content.ReadAsStringAsync());
                }
                return JsonConvert.DeserializeObject<ApiErrorResult<User>>(await response.Content.ReadAsStringAsync());

            } catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(),400);
            }
        }

        public async Task<ApiResult<bool>> RegisterUser(RegisterRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ServerHostIP"]);

                var json = JsonConvert.SerializeObject(request);
                var httpContext = new StringContent(json, Encoding.UTF8, "application/json");

                var path = "/api/Users/Register";
                var secretKey = _configuration["SecretKey"];
                var generateToken = HMACSHA256(path, secretKey);
                httpContext.Headers.Add("Token", generateToken);

                var response = await client.PostAsync(path, httpContext);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
                return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
            } 
            catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }
        }

        public async Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ServerHostIP"]);

                var json = JsonConvert.SerializeObject(request);
                var httpContext = new StringContent(json, Encoding.UTF8, "application/json");

                var path = $"/api/Users/{id}";
                var secretKey = _configuration["SecretKey"];
                var generateToken = HMACSHA256(path, secretKey);
                httpContext.Headers.Add("Token", generateToken);

                var response = await client.PutAsync(path, httpContext);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
                return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
            } 
            catch(Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ServerHostIP"]);

                var secretKey = _configuration["SecretKey"];
                var path = $"/api/Users/{id}";
                var generateToken = HMACSHA256(path, secretKey);
                client.DefaultRequestHeaders.Add("Token", generateToken);

                var response = await client.DeleteAsync(path);
                var body = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);

                return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);

            } catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }
        }

        public async Task<ApiResult<UserVm>> GetById(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration["ServerHostIP"]);

                var secretKey = _configuration["SecretKey"];
                var path = $"/api/Users/{id}";
                var generateToken = HMACSHA256(path, secretKey);
                client.DefaultRequestHeaders.Add("Token", generateToken);

                var response = await client.GetAsync(path);
                var body = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<ApiSuccessResult<UserVm>>(body);

                return JsonConvert.DeserializeObject<ApiErrorResult<UserVm>>(body);
            } catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }

        }
    }
}
