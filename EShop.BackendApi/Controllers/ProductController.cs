using EShop.Application.Interfaces;
using EShop.Application.Services.Catalog.Products;
using EShop.BackendApi.Utils;
using EShop.Data.Entities;
using EShop.ViewModels.Catalog.ProductImages;
using EShop.ViewModels.Catalog.Products;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _ProductService;
        private readonly IEShopAuthentication _authenticate;

        public ProductController(IProductService ProductService, IEShopAuthentication authenticate)
        {
            _ProductService = ProductService;
            _authenticate = authenticate;
        }

        /// <summary>
        /// GetAllPaging: Lấy toàn bộ thông tin
        /// </summary>
        /// <param name="languageId">Mã sản phẩm</param>
        /// <param name="request">Body</param>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            //_authenticate.Authentication(Request);
            var products = await _ProductService.GetAllPaging(request);
            if (products == null)
                return BadRequest("Cannot find product");
            return Ok(products);
        }

        /// <summary>
        /// GetById: Lấy sản phẩm theo mã
        /// </summary>
        /// <param name="productId">mã sản phẩm</param>
        /// <param name="languageId">ngôn ngữ</param>
        /// <returns></returns>
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
           // _authenticate.Authentication(Request);
            var product = await _ProductService.GetById(productId, languageId);
            if (product == null)
                return BadRequest("Can' not find Product");
            return Ok(product);
        }

        /// <summary>
        /// Create: Tạo mới một sản phẩmm
        /// </summary>
        /// <param name="request">Body</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
          //  _authenticate.Authentication(Request);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _ProductService.Create(request);
            if (productId == 0)
                return BadRequest();
            var product = await _ProductService.GetById(productId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        /// <summary>
        /// Update: Chỉnh sửa một sản phẩm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            _authenticate.Authentication(Request);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedReSult = await _ProductService.Update(request);
            if (affectedReSult.ResultObj == 0)
                return BadRequest();
            return Ok();
        }

        /// <summary>
        /// Delete: Xóa một sản phẩm
        /// </summary>
        /// <param name="productId">Mã sản phẩm</param>
        /// <returns></returns>
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            _authenticate.Authentication(Request);
            var affectedReSult = await _ProductService.Delete(productId);
            if (affectedReSult == 0)
                return BadRequest();
            return Ok();
        }

        /// <summary>
        /// UpdatePrice: Cập nhật giá của một sản phẩm
        /// </summary>
        /// <param name="productId">mã sản phẩm</param>
        /// <param name="newPrice">giá mới</param>
        /// <returns></returns>
        [HttpPatch("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            _authenticate.Authentication(Request);
            var IsSucessFul = await _ProductService.UpdatePrice(productId, newPrice);
            if (IsSucessFul)
                return Ok();
            return BadRequest();
        }

        /// <summary>
        /// CreateImage: Tạo mới một hình ảnh
        /// </summary>
        /// <param name="productId">mã sản phẩm</param>
        /// <param name="request">body</param>
        /// <returns></returns>
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            _authenticate.Authentication(Request);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _ProductService.AddImage(productId, request);
            if (imageId == 0)
                return BadRequest();
            var image = await _ProductService.GetImageById(imageId);
            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        /// <summary>
        /// UpdateImage: Cập nhật hình ảnh sản phẩm
        /// </summary>
        /// <param name="imageId">Mã sản phẩm</param>
        /// <param name="request">Body</param>
        /// <returns></returns>
        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            _authenticate.Authentication(Request);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _ProductService.UpdateImage(imageId, request);
            if (result == 0)
                return BadRequest();
            return Ok();
        }

        /// <summary>
        /// DeleteImage: Xóa một hình ảnh sản phẩm
        /// </summary>
        /// <param name="imageId">Mã hình ảnh</param>
        /// <returns></returns>
        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            _authenticate.Authentication(Request);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _ProductService.RemoveImage(imageId);
            if (result == 0)
                return BadRequest();
            return Ok();
        }

        /// <summary>
        /// GetImageById: Lấy một mã hình ảnh sản phẩm
        /// </summary>
        /// <param name="imageId">Mã sản phẩm</param>
        /// <returns></returns>
        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int imageId)
        {
            _authenticate.Authentication(Request);
            var image = await _ProductService.GetImageById(imageId);
            if (image == null)
                return BadRequest("Can' not find Product");
            return Ok(image);
        }
    }
}