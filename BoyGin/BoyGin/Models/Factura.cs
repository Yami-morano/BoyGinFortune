using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class Factura
    {
        public int idFactura { get; set; }
        public int idCliente { get; set; }

        public DateTime fechaFactura { get; set; }

    }
}