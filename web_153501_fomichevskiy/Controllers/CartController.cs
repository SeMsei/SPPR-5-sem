using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_153501_fomichevskiy.Models;
using web_153501_fomichevskiy.Services.ProductService;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_153501_fomichevskiy.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private IProductService _productService;
		private Cart _cart;

		public CartController(IProductService productService, Cart cart)
		{
			_productService = productService;
			_cart = cart;
		}

		public IActionResult Index()
		{
			return View(_cart);
		}

		[Route("[controller]/add/{id:int}")]
		public async Task<ActionResult> Add(int id, string returnUrl)
		{
			var data = await _productService.GetProductByIdAsync(id);
			if (data.Success)
			{
				_cart.AddToCart(data.Data!);
			}
			return Redirect(returnUrl);
		}

		public IActionResult RemoveItem(int id, string redirectUrl)
		{
			_cart.RemoveItems(id);
			return Redirect(redirectUrl);
		}
	}
}
