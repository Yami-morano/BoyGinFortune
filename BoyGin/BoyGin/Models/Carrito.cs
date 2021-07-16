using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class Carrito
    {
       
        public int idProducto { get; set; }
        public string nombreProducto { get; set; }
        public int cantidadRequerida { get; set; }
        public int stockProducto { get; set; }
        public double precioProducto { get; set; }

        public Carrito()
        {

        }
        public Carrito(int idProducto, string nombreProducto, int cantidadRequerida, int stockProducto, int precioProducto)
        {
            this.idProducto = idProducto;
            this.nombreProducto = nombreProducto;
            this.cantidadRequerida = cantidadRequerida;
            this.stockProducto = stockProducto;
            this.precioProducto = precioProducto;
        }
            
        }
    
}