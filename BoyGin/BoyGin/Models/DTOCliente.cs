using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class DTOCliente
    {
        public int idCliente { get; set; }
        public string nombreCliente { get; set; }
        public string apellidoCliente { get; set; }
        public string DNI { get; set; }
        public string domicilio { get; set; }
        public int numeroDomicilio { get; set; }
        public string localidad { get; set; }
        public int idProvincia { get; set; }
        public string nombreProvincia { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string idUsuario { get; set; }
        public string estado { get; set; }
    }
}
