using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSkiOprema.Models;
using MvcSkiOprema.ViewModels;

namespace MvcSkiOprema.Controllers
{
    public class ShoppingCartController : Controller
    {
        SkiOpremaDB storeDB = new SkiOpremaDB();

        //
        // GET: /ShoppingCart/

        [Authorize]
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Oprema/AddToCart/5

        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var dodanaOprema = storeDB.Oprema.Single(oprema => oprema.Id == id);
            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(dodanaOprema);
            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            string opremaNaziv = storeDB.Carts
                .Single(item => item.OpremaId == id).Oprema.Naziv;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(opremaNaziv) +
                    " oprema je uklonjena iz Vaše košarice.",
                CartTotal = cart.GetTotal(),
                DeleteId = id
            };
            return Json(results);
        }

        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            return PartialView("CartSummary");
        }
    }
}
