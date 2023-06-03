using EShop.Data.Entities;
using EShop.ViewModels.Common;
using EShop.ViewModels.Systems.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EShop.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiErrorResult<string>> Login(LoginRequest request)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("https://localhost:5001");
                var response = await client.PostAsync("/api/Users/Login", httpContent);

                //Check nếu lấy được data thì trả về Success nếu không thì trả về Error
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(await response.Content.ReadAsStringAsync());
                }
                return JsonConvert.DeserializeObject<ApiErrorResult<string>>(await response.Content.ReadAsStringAsync());
            } catch (Exception ex)
            {
                return JsonConvert.DeserializeObject<ApiErrorResult<string>>(ex.ToString());
            }
        }
    }
}