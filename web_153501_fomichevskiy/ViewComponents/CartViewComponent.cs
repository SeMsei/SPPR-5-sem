using System;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;

namespace web_153501_fomichevskiy.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public Cart Cart { get; set; }

        public CartViewComponent(Cart cart)
        {
            Cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(Cart);
        }
    }
}
