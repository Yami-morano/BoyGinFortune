using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class CompraMateriaPrima
    {
        public int idCompraMateriaPrima { get; set; }
        [Required(ErrorMessage = "Seleccione la materia prima.")]
        public int idMateriaPrima { get; set; }
        [Required(ErrorMessage = "seleccione el proveedor.")]
        public int idProveedor { get; set; }

        [DataType(DataType.Date)]
        public DateTime fechaCompra { get; set; }
        [Required(ErrorMessage = "Indique la cantidad.")]
        [RegularExpression("[0-9]{1,10}$", ErrorMessage = "el campo cantidad no contiene información válida")]
        public int cantidadComprada { get; set; }
        
    }
}