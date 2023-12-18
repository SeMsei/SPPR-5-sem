using System;
using System.Text.Json.Serialization;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using web_153501_fomichevskiy.Extensions;

namespace web_153501_fomichevskiy.Services.CartService
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
            .HttpContext?.Session;
            SessionCart cart = session?.Get<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession? Session { get; set; }

        public override void AddToCart(VideoGame videoGame)
        {
            base.AddToCart(videoGame);
            Session?.Set("Cart", this);
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            Session?.Set<SessionCart>("Cart", this);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            Session?.Remove("Cart");
        }
    }
}
