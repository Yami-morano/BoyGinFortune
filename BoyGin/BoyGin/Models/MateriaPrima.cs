using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class MateriaPrima
    {
        public int idMateriaPrima { get; set; }
        [Required (ErrorMessage = "El campo descripcion es requerido")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "el campo descripción no contiene información válida")]
        public string descripcionMateriaPrima { get; set; }
        public int stockMateriaPrima { get; set; }
        public bool estado { get; set; }

    }
}