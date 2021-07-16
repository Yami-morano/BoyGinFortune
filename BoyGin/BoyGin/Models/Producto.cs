using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class Producto
    {
        public int idProducto { get; set; }
        [Required(ErrorMessage = "El campo nombre es obligatorio.")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "el campo nombre no contiene información válida")]
        public string nombreProducto { get; set; }
        [Required(ErrorMessage = "El campo descripción es obligatorio.")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "el campo descripción no contiene información válida")]
        public string descripcion { get; set; }
        [Required(ErrorMessage = "Seleccione el tipo de producto.")]
        public int idTipoProducto { get; set; }
        [Required(ErrorMessage = "Debe ingresar el precio.")]
        [RegularExpression("^[0-9]{1,9}(?:.[0-9]{1,2})?$", ErrorMessage = "el campo precio no contiene información válida")]
        public double precioProducto { get; set; }
        public int stockProducto { get; set; }
        public int idProduccion { get; set; }
        public bool estado { get; set; }

    }
}