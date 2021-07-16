using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class DTOMateriaPrima
    {
        public int idMateriaPrima { get; set; }
        public string descripcionMateriaPrima { get; set; }
        public int stockMateriaPrima { get; set; }
        public int idProveedor { get; set; }
        public string nombreProveedor { get; set; }
        public bool estado { get; set; }




    }
}