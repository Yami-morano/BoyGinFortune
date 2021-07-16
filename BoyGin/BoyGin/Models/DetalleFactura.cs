using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class DetalleFactura
    {
       
        public int idDetalleFactura { get; set; }
        public int idFactura { get; set; }
        public int idProducto { get; set; }
        public double precioUnitario { get; set; }
        public int cantidad { get; set; }
        public Nullable<double> Subtotal { get; set; }
        public Nullable<double> Iva { get; set; }
        public Nullable<double> Total { get; set; }

        public DetalleFactura(int idProd, double precio, int cantidad)
        {
            this.idProducto = idProd;
            this.precioUnitario = precio;
            this.cantidad = cantidad;
        }
    }
}