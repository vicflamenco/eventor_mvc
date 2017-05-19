using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eventor.Models.App;
using Microsoft.AspNet.Identity;

namespace Eventor.Controllers
{
    public class EventosController : Controller
    {
        private EventorDbContext db = new EventorDbContext();

        [Authorize(Roles = "Organizador")]
        // GET: Eventos
        public ActionResult Index()
        {

            var username = User.Identity.GetUserName();

            var eventos = db.Eventos.Where(e=> e.UserName == username).Include(e => e.Contacto).OrderByDescending(e=>e.Inicio);

            return View(eventos.ToList());
        }
        [Authorize(Roles = "Participante, Organizador")]
        public ActionResult Explorar()
        {
            if (User.IsInRole("Organizador"))
            {
                return RedirectToAction("Index");
            }
            else
            {
                var usuario = User.Identity.GetUserName();
                var eventos = db.Eventos.ToList();
                var ProximosEventos = new List<Evento>();
                var EventosHistoricos = new List<Evento>();

                foreach (var evento in eventos)
                    if (evento.Participantes.Any(p => p.Correo == usuario))
                    {
                        if (DateTime.Compare(evento.Final, DateTime.Now) > 0)
                            ProximosEventos.Add(evento);
                        else
                            EventosHistoricos.Add(evento);
                    }
                ViewBag.ProximosEventos = ProximosEventos;
                ViewBag.EventosHistoricos = EventosHistoricos;

                return View(eventos);
            }
        }
        [Authorize(Roles = "Organizador, Participante")]
        // GET: Eventos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }

            ViewBag.Invitados = evento.Participantes.Count();
            ViewBag.Confirmados = evento.Participantes.Where(e => e.Estado == Estado.Confirmado).Count();
            ViewBag.Cancelados = evento.Participantes.Where(e => e.Estado == Estado.Cancelado).Count();
            ViewBag.Finalizado = evento.Final.CompareTo(DateTime.Now) < 0;
            ViewBag.EntradasVendidas = 0;
            ViewBag.Ingresos = 0;


            var items = db.Items.Where(i=>i.Entrada.EventoId == id).ToList();

            if (items != null)
            {
                var total = items.Sum(i => i.Subtotal);

                var cantidad = items.Sum(i => i.Cantidad);

                ViewBag.EntradasVendidas = cantidad;
                ViewBag.Ingresos = total;
            }
            
            Session["EventoId"] = id;

            return View(evento);
        }
        
        [Authorize(Roles = "Organizador")]
        // GET: Eventos/Create
        public ActionResult Create()
        {
            var modelo = new CrearEventoModelo();
            ViewBag.UserName = User.Identity.GetUserName();
            return View(modelo);
        }

        // POST: Eventos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Organizador")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrearEventoModelo modelo)
        {
            var contacto = modelo.Contacto;
            var evento = modelo.Evento;

            if (modelo.Evento.Inicio.CompareTo(modelo.Evento.Final) > 0)
                ModelState.AddModelError("", "La fecha de inicio debe ser menor que la fecha final");

            if (ModelState.IsValid)
            {
                evento.Contacto = contacto;
                evento.UserName = User.Identity.GetUserName();
                db.Eventos.Add(evento);
                db.SaveChanges();
                return RedirectToAction("Details", new { @id = evento.EventoId });
            }
            return View(modelo);
        }
        [Authorize(Roles = "Organizador")]
        // GET: Eventos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var modelo = new CrearEventoModelo();
            modelo.Evento =  db.Eventos.Find(id);
            if (modelo.Evento == null)
            {
                return HttpNotFound();
            }
            modelo.Contacto = modelo.Evento.Contacto;

            return View(modelo);
        }
        [Authorize(Roles = "Organizador")]
        // POST: Eventos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CrearEventoModelo modelo)
        {
            if (modelo.Evento.Inicio.CompareTo(modelo.Evento.Final) > 0)
                ModelState.AddModelError("", "La fecha de inicio debe ser menor que la fecha final");
            
            if (ModelState.IsValid)
            {
                db.Entry(modelo.Contacto).State = EntityState.Modified;
                db.SaveChanges();

                modelo.Evento.ContactoId = modelo.Contacto.ContactoId;

                db.Entry(modelo.Evento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { @id = modelo.Evento.EventoId });
            }
            return View(modelo);
        }
        [Authorize(Roles = "Organizador")]
        // GET: Eventos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }
        [Authorize(Roles = "Organizador")]
        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Evento evento = db.Eventos.Find(id);
            db.Eventos.Remove(evento);
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
