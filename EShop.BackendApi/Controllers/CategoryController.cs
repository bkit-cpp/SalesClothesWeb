using EShop.Application.Catalog.CategoryTranslations;
using EShop.Application.Interfaces;
using EShop.Data.Entities;
using EShop.ViewModels.Catalog.Categories;
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
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;
        private ICategoryTranslationService _categoryTranslationService;
        private IProductInCategoryService _productInCategoryService;
        private IEShopAuthentication _authenticate;

        //   private CategoryTranslationController _categoryTranslationController;
        public CategoryController(IEShopAuthentication authentication, ICategoryService categoryService, ICategoryTranslationService categoryTranslationService, IProductInCategoryService productInCategoryService)
        {
            _categoryService = categoryService;
            _categoryTranslationService = categoryTranslationService;
            _productInCategoryService = productInCategoryService;
            _authenticate = authentication;
        }

        /// <summary>
        /// GetAll: Lấy toàn bộ phân loại sản phẩm
        /// </summary>
        /// <param name="languageId">Mã ngôn ngữ</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(string languageId)
        {
            try
            {
                //_authenticate.Authentication(Request);
                var categorys = await _categoryService.GetAllAsync(languageId);
                return Ok(categorys);
            } catch (Exception ex)
            {
                throw new BadRequestException(ex.ToString(), 401);
            }

        }

        /// <summary>
        /// GetById: Lấy loại sản phẩm theo mã
        /// </summary>
        /// <param name="languageId">mã ngôn ngữ</param>
        /// <param name="id">Mã loại sản phẩm</param>
        /// <returns></returns>
        [HttpGet("{id}/{languageId}")]
        public async Task<IActionResult> GetById(string languageId, int id)
        {
            //_authenticate.Authentication(Request);
            var category = await _categoryService.GetById(languageId, id);
            return Ok(category);
        } 

        /// <summary>
        /// Create: Tạo mới một loại sản phẩm
        /// </summary>
        /// <param name="request">Data json</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateRequest request)
        {
            //_authenticate.Authentication(Request);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryService.CreateAsync(request);

            if (result.ResultObj == 0)
                return BadRequest();            
            
            var categoryIdNew = result.ResultObj;
            var category = await _categoryService.GetById(request.LanguageId, categoryIdNew);

            return CreatedAtAction(nameof(GetById), new { id = categoryIdNew }, category);
        }

        /// <summary>
        /// UpdateStatus: Sửa trạng thái hoạt đông của danh mục sản phẩm theo id
        /// </summary>
        /// <param name="id">Id sẳn phẩm</param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateStatus ([BindRequired] int id)
        {
            var result = await _categoryService.Update(id);

            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}