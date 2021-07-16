using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class VMDetalleFactura
    {
        public DetalleFactura detalleFacturaModel { get; set; }
        public Factura facturaModel { get; set; }
        public List<Producto> productoModel { get; set; }
        public List<Cliente> clienteModel { get; set; }
        public Producto productoObjVM { get; set; }
       

     

 
    }
}