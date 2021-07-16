using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class DTODetalleFactura
    {
        public int idDetalleFactura { get; set; }
        public int idFactura { get; set; }
        public int idCliente { get; set; }
        public string nombreCliente { get; set; }

        [Required(ErrorMessage = "El campo fecha es obligatorio.")]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}",ApplyFormatInEditMode =true)]
        public DateTime fechaFactura { get; set; }
        public int idProducto { get; set; }
        public string nombreProducto { get; set; }
        public double precioUnitario { get; set; }
        public int cantidad { get; set; }
        public double total { get; set; }
        public Nullable<double> Subtotal { get; set; }

        public DTODetalleFactura(int idDetalleFactura, int idFactura, int idCliente, string nombreCliente, DateTime fechaFactura, int idProducto, string nombreProducto, double precioUnitario, int cantidad, double total, double? subtotal)
        {
            this.idDetalleFactura = idDetalleFactura;
            this.idFactura = idFactura;
            this.idCliente = idCliente;
            this.nombreCliente = nombreCliente;
            this.fechaFactura = fechaFactura;
            this.idProducto = idProducto;
            this.nombreProducto = nombreProducto;
            this.precioUnitario = precioUnitario;
            this.cantidad = cantidad;
            this.total = total;
            Subtotal = subtotal;
        }

        public DTODetalleFactura() { }



    }
}