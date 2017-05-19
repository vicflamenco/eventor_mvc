using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eventor.Models.App
{
    public class Item
    {
        public int ItemId { get; set; }

        public int PedidoId { get; set; }

        [Required]
        public int EntradaId { get; set; }

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        public decimal Subtotal { get; set; }

        public virtual Entrada Entrada { get; set; }

        public virtual Pedido Pedido { get; set; }
    }
}