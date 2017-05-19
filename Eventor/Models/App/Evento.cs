using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Eventor.Models.App
{
    public class Evento
    {
        public int EventoId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Longitud máxima: 50 caracteres")]
        public string Nombre { get; set; }

        [DataType(DataType.DateTime, ErrorMessage="Fecha no válida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm}", ApplyFormatInEditMode = true)]
        public DateTime Inicio { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Fecha no válida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm}", ApplyFormatInEditMode = true)]
        public DateTime Final { get; set; }

        [StringLength(200, ErrorMessage = "Longitud máxima: 200 caracteres")]
        public string Lugar { get; set; }

        [StringLength(50, ErrorMessage = "Longitud máxima: 50 caracteres")]
        public string Organizador { get; set; }

        [Required]
        public Acceso Acceso { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Longitud máxima: 500 caracteres")]
        public string Descripcion { get; set; }

        [Required]
        public int ContactoId { get; set; }


        [Required]
        public string UserName { get; set; }


        public virtual Contacto Contacto { get; set; }


        public virtual ICollection<Entrada> Entradas { get; set; }

        public virtual ICollection<Participante> Participantes { get; set; }
    }

    public enum Acceso {
        Publico,
        Privado
    }
}