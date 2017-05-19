using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eventor.Models.App;
using ASPSnippets.GoogleAPI;
using System.Web.Script.Serialization;
using Postal;
using Microsoft.AspNet.Identity;

namespace Eventor.Controllers
{
    
    public class ParticipantesController : Controller
    {
        private EventorDbContext db = new EventorDbContext();

        [Authorize(Roles = "Organizador, Participante")]
        // GET: Participantes
        public ActionResult Index(string Participante)
        {
            int id;

            if (Session["EventoId"] == null)
                return RedirectToAction("Index", "Eventos");

            id = (int)Session["EventoId"];

            var evento = db.Eventos.Find(id);

            var participantes = db.Participantes.Include(p => p.Evento).Where(e => e.EventoId == id);
            ViewBag.NombreEvento = evento.Nombre;
            ViewBag.Finalizado = evento.Final.CompareTo(DateTime.Now) < 0;

            if (!String.IsNullOrEmpty(Participante))
                participantes = participantes.Where(p => p.Nombre.ToUpper().Contains(Participante.ToUpper()));

            return View(participantes.ToList());
        }

        [Authorize(Roles = "Organizador")]
        // GET: Participantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participante participante = db.Participantes.Find(id);
            if (participante == null)
            {
                return HttpNotFound();
            }
            return View(participante);
        }
        [Authorize(Roles = "Organizador")]
        // GET: Participantes/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.EventoId = id;
            ViewBag.NombreEvento = db.Eventos.Where(e => e.EventoId == id).Single().Nombre;
            return View();
        }
        [Authorize(Roles = "Organizador")]
        // POST: Participantes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ParticipanteId,Nombre,Apellido,Correo,Estado,EventoId")] Participante participante)
        {
            if (ModelState.IsValid)
            {
                db.Participantes.Add(participante);
                db.SaveChanges();
                SendEmail(participante.ParticipanteId,participante.EventoId);
                return RedirectToAction("Index", new { @id = participante.EventoId });
            }

            ViewBag.EventoId = new SelectList(db.Eventos, "EventoId", "Nombre", participante.EventoId);
            return View(participante);
        }
        [Authorize(Roles = "Organizador")]
        [HttpPost]
        public ActionResult SendEmail(int participanteId, int eventoId)
        {

            var participante = db.Participantes.Find(participanteId);

            var evento = db.Eventos.Find(eventoId);

            try
            {
                dynamic email = new Email("Invitacion");
                email.To = String.Format("{0} <{1}>", participante.Nombre, participante.Correo);
                email.Subject = String.Format("Invitación a {0}", evento.Nombre);
                email.From = "Eventor <eventor.udb@gmail.com>";
                email.NombreParticipante = participante.Nombre;
                email.NombreEvento = evento.Nombre;
                email.Organizador = evento.Organizador;
                email.Contacto = evento.Contacto.Nombre;
                email.Correo = evento.Contacto.Correo;
                email.UrlEvento = "http://" + Request.Url.Authority + Url.Action("Details", "Eventos", new { id = eventoId });
                email.Ubicacion = evento.Lugar;
                email.UrlConfirmar = "http://" + Request.Url.Authority + Url.Action("Index", "Entradas", new { id = eventoId });
                email.UrlCancelar = "http://" + Request.Url.Authority + Url.Action("Cancelar", "Participantes", new { id = participanteId });

                email.Send();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        [Authorize(Roles = "Organizador")]
        // GET: Participantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participante participante = db.Participantes.Find(id);
            if (participante == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventoId = participante.EventoId;
            return View(participante);
        }
        [Authorize(Roles = "Organizador")]
        // POST: Participantes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ParticipanteId,Nombre,Apellido,Correo,Estado,EventoId")] Participante participante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(participante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Participantes", new { @id = participante.EventoId });
            }
            ViewBag.EventoId = new SelectList(db.Eventos, "EventoId", "Nombre", participante.EventoId);
            return View(participante);
        }
        [Authorize(Roles = "Organizador")]
        // GET: Participantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participante participante = db.Participantes.Find(id);
            if (participante == null)
            {
                return HttpNotFound();
            }
            ViewBag.NombreEvento = db.Eventos.Find(participante.EventoId).Nombre;
            return View(participante);
        }
        [Authorize(Roles = "Organizador")]
        // POST: Participantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Participante participante = db.Participantes.Find(id);

            var eventoid = participante.EventoId;

            db.Participantes.Remove(participante);
            db.SaveChanges();
            return RedirectToAction("Index", new { @id = eventoid });
        }

        public void Autorizar()
        {
            GoogleConnect.ClientId = "979504084533-dsaiapt4m4bec0ad5sg459qdn2pe3big.apps.googleusercontent.com";
            GoogleConnect.ClientSecret = "qg0ic-W-odxIrMUS-UM2FpeO";

            string url = Request.Url.AbsoluteUri.Split('?')[0];
            url = url.Substring(0, url.Length - 9) + "ImportarContactos";

            GoogleConnect.RedirectUri = url;


            GoogleConnect.API = EnumAPI.Contacts;
            GoogleConnect.Authorize(Server.UrlEncode("https://www.google.com/m8/feeds/"));
        }
        [Authorize(Roles = "Organizador")]
        [HttpGet]
        public ActionResult ImportarContactos(string code, string error)
        {
            int id = (int)Session["EventoId"];

            if (!string.IsNullOrEmpty(code))
            {
                string json = GoogleConnect.Fetch("me", code, int.MaxValue);
                var profile = new JavaScriptSerializer().Deserialize<ImportarContactos.GoogleContacts>(json);

                var participantes = new List<ParticipanteSeleccionado>();

                foreach (var contact in profile.Feed.Entry)
                {
                    string name = contact.Title.T;
                    if (contact.GdEmail != null && !String.IsNullOrEmpty(name))
                    {
                        string email = contact.GdEmail.Find(p => p.Primary).Address;
                            
                        //var photo = contact.Link.Find(p => p.Rel.EndsWith("#photo"));
                        //string photoUrl = GoogleConnect.GetPhotoUrl(photo != null ? photo.Href : "~/default.png");

                        participantes.Add(new ParticipanteSeleccionado()
                        {
                            Participante = new Participante() {
                                Nombre = name,
                                Correo = email,
                                Estado = Estado.Invitado,
                                EventoId = id
                            },
                            Seleccionado = true
                        });
                    }
                }
                ViewBag.NombreEvento = db.Eventos.Find(id).Nombre;
                ViewBag.NumeroContactos = participantes.Count;
                return View(participantes);
            }
            if (error == "access_denied")
            {
                
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Access denied.')", true);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Confirmar(int id)
        {
            
            var participante = db.Participantes.Find(id);

            if (participante != null)
            {
                participante.Estado = Estado.Confirmado;
                db.Entry(participante).State = EntityState.Modified;
                db.SaveChanges();

                var evento = db.Eventos.Find(participante.EventoId);
                ViewBag.NombreEvento = evento.Nombre;
                ViewBag.NombreParticipante = participante.Nombre;
                ViewBag.EventoId = participante.EventoId;
                return View();
            }
            else
            {
                return View();
            }
        }

        public ActionResult Cancelar(int id)
        {
            var participante = db.Participantes.Find(id);

            if (participante != null)
            {
                participante.Estado = Estado.Cancelado;
                db.Entry(participante).State = EntityState.Modified;
                db.SaveChanges();

                var evento = db.Eventos.Find(participante.EventoId);
                ViewBag.NombreEvento = evento.Nombre;
                ViewBag.NombreParticipante = participante.Nombre;
                ViewBag.EventoId = participante.EventoId;
                return View();
            }
            else
            {
                return View();
            }
        }

        [Authorize(Roles = "Organizador")]
        [HttpPost]
        public ActionResult InvitarContactos(List<ParticipanteSeleccionado> participantes)
        {
            
            foreach (var participante in participantes)
            {
                
                bool existe = db.Participantes.Where(
                    p => 
                        p.Correo == participante.Participante.Correo &&
                        p.EventoId == participante.Participante.EventoId
                    ).Count() > 0;

                if (!existe && participante.Seleccionado)
                    db.Participantes.Add(participante.Participante);
            }

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
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
