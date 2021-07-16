using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class DTOProveedor
    {
        public int idProveedor { get; set; }
        public string nombre_razonSocial { get; set; }
        public string apellido_TipoSoc { get; set; }
        public string direccion { get; set; }
        public int numeroDireccion { get; set; }
        public string localidad { get; set; }
        public int idProvincia { get; set; }
        public string nombreProvincia { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public bool estado { get; set; }
    }
}