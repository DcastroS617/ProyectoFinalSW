using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoFinalSW.Models;

namespace ProyectoFinalSW.Controllers
{
    public class AerolineaController : Controller
    {
        private VVuelosEntities db = new VVuelosEntities();

        // GET: Aerolinea
        public ActionResult Index()
        {
            var aerolineas = db.Aerolineas.Include(a => a.Consecutivo);
            return View(aerolineas.ToList());
        }

        // GET: Aerolinea/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aerolinea aerolinea = db.Aerolineas.Find(id);
            if (aerolinea == null)
            {
                return HttpNotFound();
            }
            return View(aerolinea);
        }

        // GET: Aerolinea/Create
        public ActionResult Create()
        {
            ViewBag.ConsecutivoId = new SelectList(db.Consecutivoes, "Id", "Descripcion");
            return View();
        }

        // POST: Aerolinea/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Logo,ConsecutivoId")] Aerolinea aerolinea)
        {
            if (ModelState.IsValid)
            {
                db.Aerolineas.Add(aerolinea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ConsecutivoId = new SelectList(db.Consecutivoes, "Id", "Descripcion", aerolinea.ConsecutivoId);
            return View(aerolinea);
        }

        // GET: Aerolinea/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aerolinea aerolinea = db.Aerolineas.Find(id);
            if (aerolinea == null)
            {
                return HttpNotFound();
            }
            ViewBag.ConsecutivoId = new SelectList(db.Consecutivoes, "Id", "Descripcion", aerolinea.ConsecutivoId);
            return View(aerolinea);
        }

        // POST: Aerolinea/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Logo,ConsecutivoId")] Aerolinea aerolinea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aerolinea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ConsecutivoId = new SelectList(db.Consecutivoes, "Id", "Descripcion", aerolinea.ConsecutivoId);
            return View(aerolinea);
        }

        // GET: Aerolinea/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aerolinea aerolinea = db.Aerolineas.Find(id);
            if (aerolinea == null)
            {
                return HttpNotFound();
            }
            return View(aerolinea);
        }

        // POST: Aerolinea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Aerolinea aerolinea = db.Aerolineas.Find(id);
            db.Aerolineas.Remove(aerolinea);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
