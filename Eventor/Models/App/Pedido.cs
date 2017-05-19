using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eventor.Models.App
{
    public class Pedido
    {
        public Pedido()
        {
            Items = new HashSet<Item>();
        }
        public int PedidoId { get; set; }

        public decimal Total { get; set; }

        [Required]
        public string UserName { get; set; }

        [DataType(DataType.DateTime, ErrorMessage="Fecha no válida")]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime FechaHora { get; set; }

        public virtual ICollection<Item> Items { get; set; }

    }
}