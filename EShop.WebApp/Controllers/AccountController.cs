using EShop.ApiIntegration;
using EShop.ViewModels.Systems.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApiClient _userApiclient;
        public AccountController(IUserApiClient userApiClient)
        {
            _userApiclient = userApiClient;
        }
        [HttpGet]
        public async Task< IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            var result = await _userApiclient.RegisterUser(register);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiclient.Login(request);
            if (result.ResultObj == null)
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu sai!");
                return View();
            }
            return RedirectToAction("Index", "Home");

        }
    }
}
