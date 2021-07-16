using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class ListaPorTipoProducto
    {
        
        public string nombreProducto { get; set; }
       
        public string descripcion { get; set; }
        [Required(ErrorMessage = "Seleccione un tipo de producto.")]
        public int idTipoProducto { get; set; }
        public string nombreTipo { get; set; }
        public double precioProducto { get; set; }
        public int stockProducto { get; set; }

        public List<Producto> ListaProducto { get; set; }

        public List<TipoProducto> tiposLista { get; set; }

        public List<DTOProducto> cantidadTipo { get; set; }
    }
}