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
    [Authorize(Roles = "Participante")]
    public class PedidosController : Controller
    {
        EventorDbContext db = new EventorDbContext();
        
        public ActionResult Index()
        {
            var username = User.Identity.GetUserName();
            return View(db.Pedidos.Where(p => p.UserName == username).OrderByDescending(p => p.FechaHora).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.Items = pedido.Items.ToList();

            return View(pedido);
        }

        public ActionResult FinalizarCompra()
        {
            using (var db = new EventorDbContext())
            {
                var items = (List<Item>)Session["Items"];

                if (items != null)
                {
                    decimal total = 0;
                    foreach (var item in items)
                    {
                        total += item.Subtotal;
                    }

                    var pedido = new Pedido ()
                    {
                        FechaHora = DateTime.Now,
                        UserName = User.Identity.GetUserName(),
                        Total = total
                    };
                    
                    db.Pedidos.Add(pedido);
                    db.SaveChanges();

                    db.Entry(pedido).State = EntityState.Detached;

                    int id = int.Parse(db.Pedidos
                            .OrderByDescending(p => p.FechaHora)
                            .Select(r => r.PedidoId)
                            .First().ToString());

                    var pedidoAgregado = db.Pedidos.Find(id);
                    Participante participante = null;
                    var user = User.Identity.GetUserName();

                    foreach (var item in items)
                    {
                        pedidoAgregado.Items.Add(new Item
                        {
                            Cantidad = item.Cantidad,
                            EntradaId = item.EntradaId,
                            PedidoId = id,
                            Subtotal = item.Subtotal,
                            Precio = item.Precio
                        });

                        participante = db.Participantes.Where(p=>p.EventoId == item.Entrada.EventoId && p.Correo == user).Single();
                        participante.Estado = Estado.Confirmado;
                        db.Entry(participante).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }
    }
}
