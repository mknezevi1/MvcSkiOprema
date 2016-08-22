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
    public class ProizvodaciController : Controller
    {
        private SkiOpremaDB db = new SkiOpremaDB();

        //
        // GET: /Proizvodaci/

        public ActionResult Index()
        {
            return View(db.Proizvodaci.ToList());
        }

        //
        // GET: /Proizvodaci/Details/5

        public ActionResult Details(long id = 0)
        {
            Proizvodac proizvodac = db.Proizvodaci.Find(id);
            if (proizvodac == null)
            {
                return HttpNotFound();
            }
            return View(proizvodac);
        }

        //
        // GET: /Proizvodaci/Create

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Proizvodaci/Create

        [HttpPost]
        [Authorize(Roles="admin")]
        public ActionResult Create(Proizvodac proizvodac)
        {
            if (ModelState.IsValid)
            {
                db.Proizvodaci.Add(proizvodac);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(proizvodac);
        }

        //
        // GET: /Proizvodaci/Edit/5

        [Authorize(Roles = "admin")]
        public ActionResult Edit(long id = 0)
        {
            Proizvodac proizvodac = db.Proizvodaci.Find(id);
            if (proizvodac == null)
            {
                return HttpNotFound();
            }
            return View(proizvodac);
        }

        //
        // POST: /Proizvodaci/Edit/5

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Proizvodac proizvodac)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proizvodac).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proizvodac);
        }

        //
        // GET: /Proizvodaci/Delete/5

        [Authorize(Roles = "admin")]
        public ActionResult Delete(long id = 0)
        {
            Proizvodac proizvodac = db.Proizvodaci.Find(id);
            if (proizvodac == null)
            {
                return HttpNotFound();
            }
            return View(proizvodac);
        }

        //
        // POST: /Proizvodaci/Delete/5

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            Proizvodac proizvodac = db.Proizvodaci.Find(id);
            db.Proizvodaci.Remove(proizvodac);
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