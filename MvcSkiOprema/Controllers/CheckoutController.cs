using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSkiOprema.Models;

namespace MvcSkiOprema.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        SkiOpremaDB storeDB = new SkiOpremaDB();

        //
        // GET: /Checkout/KreiranjeRezervacije

        public ActionResult KreiranjeRezervacije()
        {
            //ispitaj jel kosarica prazna i ako je vrati korisnika natrag na pregled kosarice
            var cart = ShoppingCart.GetCart(this.HttpContext);
            if (cart.GetCartItems().Count == 0)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            return View();
        }

        //
        // POST: /Checkout/KreiranjeRezervacije

        [HttpPost]
        public ActionResult KreiranjeRezervacije(FormCollection values)
        {
            var rezervacija = new Rezervacija();
            TryUpdateModel(rezervacija);
            try
            {
                var cart = ShoppingCart.GetCart(this.HttpContext);
                //datumOd mora biti >= danasnjem
                if (rezervacija.DatumOd < DateTime.Today)
                {
                    ModelState.AddModelError("", "Datum posudbe mora biti veći ili jednak današnjem");
                    return View(rezervacija);
                }
                //datumDo mora biti >= DatumOd
                if (rezervacija.DatumDo < rezervacija.DatumOd)
                {
                    ModelState.AddModelError("", "Datum vraćanja mora biti veći ili jednak datumu posudbe");
                    return View(rezervacija);
                }
                rezervacija.UserId = User.Identity.Name;
                int brojDana = (int)(rezervacija.DatumDo - rezervacija.DatumOd).TotalDays + 1;
                rezervacija.UkupnaCijena = (float)cart.GetTotal() * brojDana;
                //Save Order
                storeDB.Rezervacije.Add(rezervacija);
                storeDB.SaveChanges();
                //Process the order
                cart.KreiranjeStavkiRezervacije(rezervacija);
                return RedirectToAction("Zavrsetak", new { id = rezervacija.Id });
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(rezervacija);
            }
        }

        //
        // GET: /Checkout/Complete

        public ActionResult Zavrsetak(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Rezervacije.Any(
                o => o.Id == id &&
                o.UserId == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

    }
}
