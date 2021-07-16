using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class Produccion_MateriasPrimas
    {
       
        public int idProdu_MatPrima { get; set; }
        public int idProduccion { get; set; }
        [Required(ErrorMessage = "Seleccione la materia prima.")]
        public int idMateriasPrimas { get; set; }
        [Required(ErrorMessage = "Indique la cantidad utilizada.")]
        [RegularExpression("[0-9]{1,9}$", ErrorMessage = "el campo número no contiene información válida")]
        public int cantidadMateriaPrimaUsada { get; set; }
        [Required(ErrorMessage = "Seleccione el producto.")]
        public int idProducto { get; set; }
    }
}