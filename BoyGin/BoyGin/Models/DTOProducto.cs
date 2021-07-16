using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class DTOProducto
    {
        public int idProducto { get; set; }
        public string nombreProducto { get; set; }
        public string descripcion { get; set; }
        public int idTipoProducto { get; set; }
        public string nombreTipo { get; set; }
        public double precioProducto { get; set; }
        public int stockProducto { get; set; }
        public bool estado { get; set; }


    }
}