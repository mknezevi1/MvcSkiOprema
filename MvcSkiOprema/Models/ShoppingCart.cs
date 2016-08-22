using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcSkiOprema.Models
{
    public partial class ShoppingCart
    {
        SkiOpremaDB storeDB = new SkiOpremaDB();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Oprema oprema)
        {
            // Get the matching cart and album instances
            var cartItem = storeDB.Carts.SingleOrDefault(
                c => c.Id == ShoppingCartId
                && c.OpremaId == oprema.Id);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    OpremaId = oprema.Id,
                    Id = ShoppingCartId,
                };
                storeDB.Carts.Add(cartItem);
            }
            storeDB.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            var cartItem = storeDB.Carts.Single(
                cart => cart.Id == ShoppingCartId
                && cart.OpremaId == id);

            if (cartItem != null)
            {
                storeDB.Carts.Remove(cartItem);
                storeDB.SaveChanges();
            }
            return id;
        }

        public void EmptyCart()
        {
            var cartItems = storeDB.Carts.Where(
                cart => cart.Id == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                storeDB.Carts.Remove(cartItem);
            }
            storeDB.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return storeDB.Carts.Where(
                cart => cart.Id == ShoppingCartId).ToList();
        }

        public decimal GetTotal()
        {
            try
            {
                decimal? total = (decimal)(from cartItems in storeDB.Carts
                                           where cartItems.Id == ShoppingCartId
                                           select cartItems.Oprema.Cijena).Sum();

                return total ?? decimal.Zero;
            }
            catch
            {
                return 0;
            }
        }

        public long KreiranjeStavkiRezervacije(Rezervacija rezervacija)
        {
            var cartItems = GetCartItems();
            foreach (var item in cartItems)
            {
                var stavkaRezervacije = new StavkaRezervacije
                {
                    RezervacijaId = rezervacija.Id,
                    OpremaId = item.OpremaId
                };
                storeDB.StavkeRezervacije.Add(stavkaRezervacije);
            }
            storeDB.SaveChanges();
            EmptyCart();
            return rezervacija.Id;
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }
    }
}