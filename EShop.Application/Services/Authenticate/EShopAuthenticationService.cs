using EShop.Application.Interfaces;
using EShop.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EShop.Application.Authenticate
{
    public class EShopAuthenticationService : IEShopAuthentication
    {
        private readonly AppSetting _appSettings;

        public  EShopAuthenticationService(AppSetting setting)
        {
            _appSettings = setting;
        }

        public void Authentication(HttpRequest request)
        {
            if (!request.Headers.ContainsKey("Token"))
            {
                throw new BadRequestException(MessageResponseConstants.GetMessage(MessageKey.TOKEN_NOT_FOUND), 203);
            }

            string Token = request.Headers.GetCommaSeparatedValues("Token").First();
            string path = request.Path;
            string query = request.QueryString + "";
            string pathAndQuery = path + query;
            //Configuration.GetSection("SecretKey").Value [Lấy giá trị của SecretKey trong file appsettings.json

            string generateToken = GernerateToken(pathAndQuery);
            if (!generateToken.Equals(Token))
            {
                throw new BadRequestException(MessageResponseConstants.GetMessage(MessageKey.INVALID_TOKEN), 202);
            }


        }

        private string GernerateToken(string pathAndQuery)
        {
            return HMACSHA256(pathAndQuery, _appSettings.SecretKey);
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
    }
}
