using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class VMProduccion
    {
        public Produccion_MateriasPrimas produccionmodel { get; set; }
        public HistorialProduccion historialProduModel { get; set; }
        public List<MateriaPrima> materiaPrimaModel { get; set; }
        public List<Producto> productoModel { get; set; }
        
        public List<Conceptos> conceptos { get; set; }

    }

    public class Conceptos
    {
        public string materiaPrimaId { get; set; }

        public int cantidadMateriaPrimaUsada { get; set; }
    }
   
  
}