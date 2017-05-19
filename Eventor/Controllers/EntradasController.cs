using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eventor.Models.App;

namespace Eventor.Controllers
{
    public class EntradasController : Controller
    {
        private EventorDbContext db = new EventorDbContext();

        [Authorize(Roles = "Organizador, Participante")]
        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var evento = db.Eventos.Where(ev => ev.EventoId == id).Single();
            var entradas = evento.Entradas;

            ViewBag.NombreEvento = evento.Nombre;
            ViewBag.ConteoEntradas = evento.Entradas.Count();
            ViewBag.Finalizado = evento.Final.CompareTo(DateTime.Now) < 0;

            if (User.IsInRole("Organizador"))
                return View(entradas.ToList());
            else
                return View("Buy", entradas.ToList());
        }

        // GET: Entradas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entrada entrada = db.Entradas.Find(id);
            if (entrada == null)
            {
                return HttpNotFound();
            }
            ViewBag.NombreEvento = entrada.Evento.Nombre;
            return View(entrada);
        }

        // GET: Entradas/Create
        public ActionResult Create(int id)
        {
            ViewBag.EventoId = id;
            ViewBag.NombreEvento = db.Eventos.Find(id).Nombre;
            return View();
        }

        // POST: Entradas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EntradaId,Nombre,Precio,Cupo,CantidadMinima,CantidadMaxima,Descripcion,FechaLimite,EventoId")] Entrada entrada)
        {
            if (ModelState.IsValid)
            {
                db.Entradas.Add(entrada);
                db.SaveChanges();
                return RedirectToAction("Index", new { @id = entrada.EventoId});
            }

            ViewBag.EventoId = entrada.EventoId;
            return View(entrada);
        }

        // GET: Entradas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Entrada entrada = db.Entradas.Find(id);

            if (entrada == null)
            {
                return HttpNotFound();
            }

            ViewBag.EventoId = entrada.EventoId;
            ViewBag.NombreEvento = db.Eventos.Find(entrada.EventoId).Nombre;
            
            return View(entrada);
        }

        // POST: Entradas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EntradaId,Nombre,Precio,Cupo,CantidadMinima,CantidadMaxima,Descripcion,FechaLimite,EventoId")] Entrada entrada)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entrada).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { @id = entrada.EventoId });
            }
            return View(entrada);
        }

        // GET: Entradas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entrada entrada = db.Entradas.Find(id);
            if (entrada == null)
            {
                return HttpNotFound();
            }
            ViewBag.NombreEvento = entrada.Evento.Nombre;
            return View(entrada);
        }

        // POST: Entradas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entrada entrada = db.Entradas.Find(id);
            db.Entradas.Remove(entrada);
            db.SaveChanges();
            return RedirectToAction("Index", new { @id = entrada.EventoId });
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
