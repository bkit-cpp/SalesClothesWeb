using EShop.AdminApp.Services;
using EShop.ViewModels.Systems.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.AdminApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;

        public UserController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if(!ModelState.IsValid)
            {
                return View(ModelState);
            }

            //var result = await _userApiClient.Login(request);
            ////Not have user
            //if (result.ResultObj == null)
            //{
            //    ModelState.AddModelError("", result.Message);
            //    return View();
            //}
            return RedirectToAction("Index", "Home");
        }

    }
}