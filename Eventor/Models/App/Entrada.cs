using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eventor.Models.App
{
    public class Entrada
    {
        public int EntradaId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Longitud máxima: 50 caracteres")]
        public string Nombre { get; set; }

        [Required]
        [Range(0,9999,ErrorMessage="El precio debe ser mayor o igual a cero")]
        [DisplayFormat(DataFormatString="{0:C}")]
        public decimal Precio { get; set; }

        [Required]
        [Range(1,100,ErrorMessage="La entrada debe cubrir a una persona como mínimo")]
        public int Cupo { get; set; }

        [Display(Name = "Cantidad Mínima")]
        [Required]
        [Range(1, 100, ErrorMessage = "La cantidad mínima es una unidad")]
        public int CantidadMinima { get; set; }

        [Display(Name = "Cantidad Máxima")]
        [Range(1, 100, ErrorMessage = "La cantidad máxima es 100 unidades")]
        public int CantidadMaxima { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public int EventoId { get; set; }

        public virtual Evento Evento { get; set; }
    }
}