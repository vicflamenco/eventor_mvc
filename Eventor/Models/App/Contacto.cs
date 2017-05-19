using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eventor.Models.App
{
    public class Contacto
    {
        public int ContactoId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage="Longitud máxima: 50 caracteres")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Longitud máxima: 50 caracteres")]
        public string Apellido { get; set; }

        [Required]
        [RegularExpression("^[0-9]{4}-?[0-9]{4}$", ErrorMessage="Número telefónico no válido")]
        public string Teléfono { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage="Dirección de correo electrónica no válida")]
        public string Correo { get; set; }
    }
}