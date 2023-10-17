
using EShop.ApiIntegration;
using EShop.ViewModels.Systems.Users;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EShop.WebAdmin.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserApiClient _userApiClient;

        public LoginController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public IActionResult Index() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userApiClient.Login(request);

            if (result.ResultObj == null)
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu sai!");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
