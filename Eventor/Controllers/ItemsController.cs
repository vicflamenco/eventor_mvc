using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eventor.Models.App;

namespace Eventor.Controllers
{
    public class ItemsController : Controller
    {
        EventorDbContext db = new EventorDbContext();

        public ActionResult Index()
        {
            var Items = (List<Item>)Session["Items"];
            if (Items == null)
                Items = new List<Item>();
            return View(Items);
        }

        // Se llama con Ajax desde AddToCart.js
        public ActionResult AddItemToCart(int entradaId, int cantidad, decimal precio)
        {
            try
            {
                Entrada entrada = db.Entradas.Find(entradaId);

                var Items = (List<Item>)Session["Items"];

                if (Items == null)
                    Items = new List<Item>();

                bool Encontrada = false;

                foreach (Item item in Items)
                {
                    if (item.Entrada.EntradaId == entradaId)
                    {
                        item.Cantidad += cantidad;

                        if (item.Cantidad > entrada.CantidadMaxima)
                            item.Cantidad = entrada.CantidadMaxima;

                        Encontrada = true;
                    }
                }
                if (!Encontrada)
                    Items.Add(new Item()
                    {
                        EntradaId = entradaId,
                        Entrada = entrada,
                        Cantidad = cantidad,
                        Precio = precio,
                        Subtotal = precio * cantidad
                    });

                Session["Items"] = Items;

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        public ActionResult ClearSession()
        {
            if (Session["Items"] != null)
                Session["Items"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult DeleteFromCart(int id)
        {
            var items = (List<Item>)Session["Items"];
            if (items != null)
            {
                items.Remove(items.Where(i => i.Entrada.EntradaId == id).First());
                Session["Items"] = items;
            }
                
            return RedirectToAction("Index");
        }
    }
}
