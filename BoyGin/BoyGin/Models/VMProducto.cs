using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class VMProducto
    {
        public Producto productoModel { get; set; }
        public List<TipoProducto> tipos { get; set; }

        public List<Producto> listaProducto { get; set; }
    }
}