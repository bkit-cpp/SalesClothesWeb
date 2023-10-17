using EShop.Application.Interfaces;
using EShop.Data.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class SlidesController : ControllerBase
    {
        private readonly ISlideService _slideservice;
        private readonly IEShopAuthentication _authentication;
        public SlidesController(ISlideService slideService, IEShopAuthentication authentication)
        {
            _slideservice = slideService;
            _authentication = authentication;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        { 
           // _authentication.Authentication(Request);
            var slides = await _slideservice.GetAll();
            return Ok(slides);
        }
    }
}
