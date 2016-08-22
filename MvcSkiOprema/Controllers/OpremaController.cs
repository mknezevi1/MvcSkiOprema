using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSkiOprema.Models;

namespace MvcSkiOprema.Controllers
{
    public class OpremaController : Controller
    {
        private SkiOpremaDB db = new SkiOpremaDB();

        //
        // GET: /Oprema/

        public ActionResult Index()
        {
            var oprema = db.Oprema.Include(o => o.Kategorija).Include(o => o.Proizvodac);
            return View(oprema.ToList());
        }

        //
        // GET: /Oprema/Details/5

        public ActionResult Details(long id = 0)
        {
            Oprema oprema = db.Oprema.Find(id);
            if (oprema == null)
            {
                return HttpNotFound();
            }
            return View(oprema);
        }

        //
        // GET: /Oprema/Create

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.KategorijaId = new SelectList(db.Kategorije, "Id", "Naziv");
            ViewBag.ProizvodacId = new SelectList(db.Proizvodaci, "Id", "Naziv");
            return View();
        }

        //
        // POST: /Oprema/Create

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Oprema oprema)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Oprema.Add(oprema);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "vrijednost polja dostupno ne smije biti negativan broj.");
                }
            }

            ViewBag.KategorijaId = new SelectList(db.Kategorije, "Id", "Naziv", oprema.KategorijaId);
            ViewBag.ProizvodacId = new SelectList(db.Proizvodaci, "Id", "Naziv", oprema.ProizvodacId);
            return View(oprema);
        }

        //
        // GET: /Oprema/Edit/5

        [Authorize(Roles = "admin")]
        public ActionResult Edit(long id = 0)
        {
            Oprema oprema = db.Oprema.Find(id);
            if (oprema == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategorijaId = new SelectList(db.Kategorije, "Id", "Naziv", oprema.KategorijaId);
            ViewBag.ProizvodacId = new SelectList(db.Proizvodaci, "Id", "Naziv", oprema.ProizvodacId);
            return View(oprema);
        }

        //
        // POST: /Oprema/Edit/5

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Oprema oprema)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(oprema).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "vrijednost polja dostupno ne smije biti negativan broj.");
                }
            }
            ViewBag.KategorijaId = new SelectList(db.Kategorije, "Id", "Naziv", oprema.KategorijaId);
            ViewBag.ProizvodacId = new SelectList(db.Proizvodaci, "Id", "Naziv", oprema.ProizvodacId);
            return View(oprema);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Upload()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var path = Server.MapPath("~/Images/Oprema/") + Request.Form["id"] + ".jpg";
                    try
                    {
                        //ako vec postoji slika za tu opremu, tada se ona zamjenjuje ovom novom
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        file.SaveAs(path);

                        //azuriraj atribut slika u BP
                        Oprema oprema = db.Oprema.Find(long.Parse(Request.Form["id"]));
                        oprema.Slika = Request.Form["id"] + ".jpg";
                        db.Entry(oprema).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch 
                    { 
                    
                    }
                }
            }
            return RedirectToAction("Details/" + Request.Form["id"]);
        }

        //
        // GET: /Oprema/Delete/5

        [Authorize(Roles = "admin")]
        public ActionResult Delete(long id = 0)
        {
            Oprema oprema = db.Oprema.Find(id);
            if (oprema == null)
            {
                return HttpNotFound();
            }
            return View(oprema);
        }

        //
        // POST: /Oprema/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(long id)
        {
            Oprema oprema = db.Oprema.Find(id);
            db.Oprema.Remove(oprema);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //metoda za pretrazivanje
        //usporeduje se upit q sa nazivom(modelom) te nazivima proizvodaca i kategorije
        public ActionResult Search(string q)
        {
            var oprema = db.Oprema.Include(o => o.Kategorija).Include(o => o.Proizvodac);
            if (Request.QueryString["pretrazi"] != null)
            {
                oprema = db.Oprema.Include(o => o.Kategorija).Include(o => o.Proizvodac).Where(o => o.Naziv.Contains(q) || o.Proizvodac.Naziv.Contains(q) || o.Kategorija.Naziv.Contains(q)).Take(10);
            }
            return View("Index", oprema.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}