using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class ReporteFacturacion
    {

        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }

        [JsonProperty(PropertyName = "data")]
        public double[] data { get; set; }
        //public int cantidad { get; set; }

        //public int fecha { get; set; }
        //public double total { get; set; }

        //public List<ReporteFacturacion> cantidadPorMes { get; set; }

    }
}