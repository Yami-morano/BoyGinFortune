using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class HistorialProduccion
    {
        public int idProduccion { get; set; }
        public DateTime fechaProduccion { get; set; }
        [Required(ErrorMessage = "Indique la cantidad producida.")]
        public int cantidadProducida { get; set; }
    }
}