using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class VMProveedor
    {
        public Proveedor proveedorModel { get; set; }
        public List<Provincia> provinciaModel { get; set; }
    }
}