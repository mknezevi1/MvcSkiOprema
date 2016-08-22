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
    public class KategorijeController : Controller
    {
        private SkiOpremaDB db = new SkiOpremaDB();

        //
        // GET: /Kategorije/

        public ActionResult Index()
        {
            return View(db.Kategorije.ToList());
        }

        //
        // GET: /Kategorije/Details/5

        public ActionResult Details(long id = 0)
        {
            Kategorija kategorija = db.Kategorije.Find(id);
            if (kategorija == null)
            {
                return HttpNotFound();
            }
            return View(kategorija);
        }

        //
        // GET: /Kategorije/Create

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Kategorije/Create

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Kategorija kategorija)
        {
            if (ModelState.IsValid)
            {
                db.Kategorije.Add(kategorija);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kategorija);
        }

        //
        // GET: /Kategorije/Edit/5

        [Authorize(Roles = "admin")]
        public ActionResult Edit(long id = 0)
        {
            Kategorija kategorija = db.Kategorije.Find(id);
            if (kategorija == null)
            {
                return HttpNotFound();
            }
            return View(kategorija);
        }

        //
        // POST: /Kategorije/Edit/5

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Kategorija kategorija)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kategorija).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategorija);
        }

        //
        // GET: /Kategorije/Delete/5

        [Authorize(Roles = "admin")]
        public ActionResult Delete(long id = 0)
        {
            Kategorija kategorija = db.Kategorije.Find(id);
            if (kategorija == null)
            {
                return HttpNotFound();
            }
            return View(kategorija);
        }

        //
        // POST: /Kategorije/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(long id)
        {
            Kategorija kategorija = db.Kategorije.Find(id);
            db.Kategorije.Remove(kategorija);
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