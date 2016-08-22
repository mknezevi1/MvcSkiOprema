using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSkiOprema.Models;

namespace MvcSkiOprema.Controllers
{
    public class StavkeRezervacijeController : Controller
    {
        private SkiOpremaDB db = new SkiOpremaDB();

        //
        // GET: /StavkeRezervacije/

        public ActionResult Index()
        {
            var stavkerezervacije = db.StavkeRezervacije.Include(s => s.Rezervacija).Include(s => s.Oprema);
            return View(stavkerezervacije.ToList());
        }

        //
        // GET: /StavkeRezervacije/Details/5

        public ActionResult Details(long id = 0)
        {
            StavkaRezervacije stavkarezervacije = db.StavkeRezervacije.Find(id);
            if (stavkarezervacije == null)
            {
                return HttpNotFound();
            }
            return View(stavkarezervacije);
        }

        //
        // GET: /StavkeRezervacije/Create

        public ActionResult Create()
        {
            ViewBag.RezervacijaId = new SelectList(db.Rezervacije, "Id", "UserId");
            ViewBag.OpremaId = new SelectList(db.Oprema, "Id", "Naziv");
            return View();
        }

        //
        // POST: /StavkeRezervacije/Create

        [HttpPost]
        public ActionResult Create(StavkaRezervacije stavkarezervacije)
        {
            if (ModelState.IsValid)
            {
                db.StavkeRezervacije.Add(stavkarezervacije);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RezervacijaId = new SelectList(db.Rezervacije, "Id", "UserId", stavkarezervacije.RezervacijaId);
            ViewBag.OpremaId = new SelectList(db.Oprema, "Id", "Naziv", stavkarezervacije.OpremaId);
            return View(stavkarezervacije);
        }

        //
        // GET: /StavkeRezervacije/Edit/5

        public ActionResult Edit(long id = 0)
        {
            StavkaRezervacije stavkarezervacije = db.StavkeRezervacije.Find(id);
            if (stavkarezervacije == null)
            {
                return HttpNotFound();
            }
            ViewBag.RezervacijaId = new SelectList(db.Rezervacije, "Id", "UserId", stavkarezervacije.RezervacijaId);
            ViewBag.OpremaId = new SelectList(db.Oprema, "Id", "Naziv", stavkarezervacije.OpremaId);
            return View(stavkarezervacije);
        }

        //
        // POST: /StavkeRezervacije/Edit/5

        [HttpPost]
        public ActionResult Edit(StavkaRezervacije stavkarezervacije)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stavkarezervacije).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RezervacijaId = new SelectList(db.Rezervacije, "Id", "UserId", stavkarezervacije.RezervacijaId);
            ViewBag.OpremaId = new SelectList(db.Oprema, "Id", "Naziv", stavkarezervacije.OpremaId);
            return View(stavkarezervacije);
        }

        //
        // GET: /StavkeRezervacije/Delete/5

        public ActionResult Delete(long id = 0)
        {
            StavkaRezervacije stavkarezervacije = db.StavkeRezervacije.Find(id);
            if (stavkarezervacije == null)
            {
                return HttpNotFound();
            }
            return View(stavkarezervacije);
        }

        //
        // POST: /StavkeRezervacije/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            StavkaRezervacije stavkarezervacije = db.StavkeRezervacije.Find(id);
            db.StavkeRezervacije.Remove(stavkarezervacije);
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