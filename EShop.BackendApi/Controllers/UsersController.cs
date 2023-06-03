
using EShop.Application.Interfaces;
using EShop.ViewModels.System.Users;
using EShop.ViewModels.Systems.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEShopAuthentication _authenticate;

        public UsersController(IUserService userService, IEShopAuthentication authenticate)
        {
            _userService = userService;
            _authenticate = authenticate;
        }

        /// <summary>
        /// Authentication: Login vào app admin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            _authenticate.Authentication(Request);
            var user = await _userService.Login(request);
            if (user == null)
                return BadRequest("User not found");
            //check in FE nếu tìm thấy
            return Ok(user);
        }

        /// <summary>
        /// Register: Tạo mới một user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _authenticate.Authentication(Request);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Register(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// GetAllPaging: Lấy toàn bộ danh sách user phân trang
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("Paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetListUserPagingRequest request)
        {
            _authenticate.Authentication(Request);
            var users = await _userService.GetUsersPaging(request);
            return Ok(users);
        }

        /// <summary>
        /// Delete: Xóa một user
        /// </summary>
        /// <param name="id">Mã user</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _authenticate.Authentication(Request);
            var result = await _userService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// Update: Cập nhật thông tin một User
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequest request)
        {
            _authenticate.Authentication(Request);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Update(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// Get: Láy một user theo id
        /// </summary>
        /// <param name="id">Mã người dùng</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            _authenticate.Authentication(Request);
            var users = await _userService.GetById(id);
            return Ok(users);
        }
    }
}