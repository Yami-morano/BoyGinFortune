using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class ReporteProduccion
    {
        public string nombreProducto { get; set; }
        public int cantidad { get; set; }

        public List<ReporteProduccion> cantidadProducida { get; set; }

        public int ano { get; set; }

        public List<años> añoCombo { get; set; }

    }
}