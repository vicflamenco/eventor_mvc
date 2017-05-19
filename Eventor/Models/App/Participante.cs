using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eventor.Models.App
{
    public class Participante
    {
        public int ParticipanteId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Longitud máxima: 50 caracteres")]
        public string Nombre { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Dirección de correo electrónica no válida")]
        public string Correo { get; set; }

        public Estado Estado { get; set; }

        [Required]
        public int EventoId { get; set; }

        public Evento Evento { get; set; }
    }

    public enum Estado {
        Invitado,
        Confirmado,
        Cancelado
    }
}