using EShop.ApiIntegration;
using EShop.Utilities.Constants;
using EShop.ViewModel.Sales;
using EShop.ViewModels.Common;
using EShop.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        public CartController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
      /*  public List<CartItemViewModel> GetListItems()
        {
            //HttpContext.Session.Remove(SystemConstants.CartSession);
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            if (session != null)
            {
                return JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            return new List<CartItemViewModel>();
        }
      */
         public IActionResult GetListItems()
        {
           // HttpContext.Session.Remove(SystemConstants.CartSession);
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            return Ok(currentCart);
        }
        public async Task<IActionResult> AddToCart(int id, string languageId)
        {
            var product = await _productApiClient.GetById(id, languageId);
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session!=null)
             currentCart =JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            var Item = currentCart.Find(x => x.ProductId == id);
            //int quantity = 1;
            if (Item != null)
            {
                Item.Quantity++;
            }
            else
            {
                var result = product.ResultObj;
                var cartItem = new CartItemViewModel()
                {
                    ProductId = id,
                    Description = result.Description,
                    Name = result.Name,
                    Image = result.ThumbnailImage,
                    Price = result.Price,
                    Quantity = 1
                };
                currentCart.Add(cartItem);
            }
           
            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }
        public IActionResult UpdateCart(int id, int quantity)
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            //if (currentCart.Any(x => x.ProductId == id))
            //{
            //    quantity = currentCart.First(x => x.ProductId == id).Quantity + 1;
            //}
            foreach(var item in currentCart)
            {
                if(item.ProductId==id)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item);
                        break;
                    }
                    item.Quantity = quantity;
                } 
            }

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }
        public IActionResult Checkout()
        {
           
            return View(GetCheckoutViewModel());
        }
        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel request)
        {
            var model = GetCheckoutViewModel();
            var orderDetails = new List<OrderDetailVm>();
            foreach(var item in model.CartItems)
            {
                orderDetails.Add(new OrderDetailVm()
                {
                    ProductId=item.ProductId,
                    Quantity=item.Quantity
                });
            }
            var checkoutRequest = new CheckoutRequest()
            {
                Address = request.CheckoutModel.Address,
                Name=request.CheckoutModel.Name,
                Email=request.CheckoutModel.Email,
                PhoneNumber=request.CheckoutModel.PhoneNumber,
                OrderDetail=orderDetails
            };
            TempData["SuccessMsg"] = "Order puschased successful";
            return View(model);
        }
        private CheckoutViewModel GetCheckoutViewModel()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> curentCart = new List<CartItemViewModel>();
            if (session != null)
                curentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            var checkoutVm = new CheckoutViewModel()
            {
                CartItems = curentCart,
                CheckoutModel = new CheckoutRequest()
            };
            return checkoutVm;

        }
    }
}
