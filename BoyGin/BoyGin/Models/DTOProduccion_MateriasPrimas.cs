using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class DTOProduccion_MateriasPrimas
    {
        public int idProdu_MatPrima { get; set; }
        public int idProduccion { get; set; }
        public DateTime fechaProduccion { get; set; }
        public int cantidadProducida { get; set; }
        public int idMateriasPrimas { get; set; }
        public string descripcionMateriaPrima { get; set; }
        public int cantidadMateriaPrimaUsada { get; set; }
        public int idProducto { get; set; }
        public string nombreProducto { get; set; }
    }
}