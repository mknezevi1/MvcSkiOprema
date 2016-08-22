using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcSkiOprema.Models;

namespace MvcSkiOprema.Controllers
{
    public class RezervacijeController : Controller
    {
        private SkiOpremaDB db = new SkiOpremaDB();
        SmtpClient client = null;

        //
        // GET: /Rezervacije/

        public ActionResult Index()
        {
            return View(db.Rezervacije.ToList());
        }

        //
        // GET: /Rezervacije/Details/5

        public ActionResult Details(long id = 0)
        {
            Rezervacija rezervacija = db.Rezervacije.Find(id);
            if (rezervacija == null)
            {
                return HttpNotFound();
            }
            return View(rezervacija);
        }

        //
        // GET: /Rezervacije/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Rezervacije/Create

        [HttpPost]
        public ActionResult Create(Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                db.Rezervacije.Add(rezervacija);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rezervacija);
        }

        //
        // GET: /Rezervacije/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Rezervacija rezervacija = db.Rezervacije.Find(id);
            if (rezervacija == null)
            {
                return HttpNotFound();
            }
            //popuni padajuci izbornik
            SelectList selectList = new SelectList(dohvatiMogucnostiRezervacije(), "Key", "Value");
            ViewBag.mogucnostiRezervacije = selectList;
            //sacuvaj trenutnu vrijednost od atributa Odobreno za kasniju usporedbu
            ViewBag.prethodnaVrijednost = rezervacija.Odobreno;

            //provjera preklapanja
            ViewBag.provjera = provjeriPreklapanje(id);

            return View(rezervacija);
        }

        //metoda iterira kroz rezervacije i stavke (objektna verzija SQL upita skioprema Web forms app)
        private string provjeriPreklapanje(long id) 
        {
            Rezervacija rezervacija = db.Rezervacije.Find(id);
            for (int i = 0; i < rezervacija.StavkaRezervacije.Count; i++)
            {
                foreach (Rezervacija rez in db.Rezervacije.ToList()) 
                {
                    for (int k = 0; k < rez.StavkaRezervacije.Count; k++)
                    {
                        if (rez.DatumOd <= rezervacija.DatumDo && rez.DatumDo >= rezervacija.DatumOd && rezervacija.StavkaRezervacije[i].OpremaId == rez.StavkaRezervacije[k].OpremaId && rez.Odobreno == 1 && rez.Id != rezervacija.Id) 
                        {
                            return "red";
                        }
                    }
                }
            }

            return "green";
        }

        //metoda definira i dostavlja mogucnosti za padajuci izbornik (Odobreno)
        private static Dictionary<int, string> dohvatiMogucnostiRezervacije()
        {
            Dictionary<int, string> mogucnostiRezervacije = new Dictionary<int, string>()
            {
              {1, "potvrdi"},
              {0, "odbij"}
            };
            return mogucnostiRezervacije;
        }

        //
        // POST: /Rezervacije/Edit/5

        [HttpPost]
        public ActionResult Edit(Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                string emailKlijent = rezervacija.UserId;
                int prethodnaVrijednost;
                try 
                {
                    prethodnaVrijednost = Int32.Parse(Request.Form["prethodnaVrijednost"]);
                }
                catch 
                {
                    prethodnaVrijednost = -1;
                }
                
                //ukoliko se u editiranju promijenila vrijednost Odobreno, posalji mail
                if (db.Entry(rezervacija).Entity.Odobreno != prethodnaVrijednost)
                {
                    if (db.Entry(rezervacija).Entity.Odobreno == 1)
                    {
                        string poruka = "Poštovani, ovim putem potvrđujemo Vašu rezervaciju.";
                        posaljiMail(emailKlijent, poruka);
                    }
                    if (db.Entry(rezervacija).Entity.Odobreno == 0)
                    {
                        string poruka = "Poštovani, nažalost ne možemo ispuniti Vašu rezervaciju.";
                        posaljiMail(emailKlijent, poruka);
                    }
                }

                db.Entry(rezervacija).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rezervacija);
        }

        //metoda za slanje maila
        private void posaljiMail(string primatelj, string poruka)
        {
            try
            {
                MailAddress adrPosiljatelj = new MailAddress("skioprema@test.hr", "skioprema");
                MailAddress adrPrimatelj = new MailAddress(primatelj);
                MailMessage email = new MailMessage(adrPosiljatelj, adrPrimatelj);
                email.Subject = "Skioprema obavijest";
                email.Body = poruka;
                if (client == null)
                {
                    client = new SmtpClient("localhost");
                }
                client.Send(email);
            }
            catch
            { 
            
            }
        }

        //
        // GET: /Rezervacije/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Rezervacija rezervacija = db.Rezervacije.Find(id);
            if (rezervacija == null)
            {
                return HttpNotFound();
            }
            return View(rezervacija);
        }

        //
        // POST: /Rezervacije/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            Rezervacija rezervacija = db.Rezervacije.Find(id);
            db.Rezervacije.Remove(rezervacija);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}