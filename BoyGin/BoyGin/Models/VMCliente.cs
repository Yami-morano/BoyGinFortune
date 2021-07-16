using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class VMCliente
    {
        public Cliente clienteModel { get; set; }
        public List<Provincia> provinciaModel { get; set; }
        public List<RegisterViewModel> usuarioModel { get; set; }

        public List<Cliente> clienteLista { get; set; }

    }
}