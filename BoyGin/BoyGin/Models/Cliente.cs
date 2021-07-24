using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class Cliente
    {
        public int idCliente { get; set; }

        [Required(ErrorMessage = "El campo nombre es obligatorio.")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "el campo nombre no contiene información válida")]
        public string nombreCliente { get; set; }

        [Required(ErrorMessage = "El campo apellido es obligatorio.")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "el campo apellido no contiene información válida")]
        public string apellidoCliente { get; set; }
        [Required(ErrorMessage = "El campo DNI es obligatorio.")]
        [RegularExpression("[0-9]{7,8}$", ErrorMessage = "el campo DNI no contiene información válida. Ingrese el número sin puntos")]
        public string DNI { get; set; }
        [Required(ErrorMessage = "El campo domicilio es obligatorio.")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "el campo domicilio no contiene información válida")]
        public string domicilio { get; set; }

        [Required(ErrorMessage = "El campo número es obligatorio.")]
        [RegularExpression("[0-9]{1,6}$", ErrorMessage = "el campo número no contiene información válida")]
        public int numeroDomicilio { get; set; }

        [Required(ErrorMessage = "El campo localidad es obligatorio.")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "el campo Localidad no contiene información válida")]
        public string localidad { get; set; }
        [Required(ErrorMessage = "Debe selecionar la provincia.")]
        public int idProvincia { get; set; }

        [Required(ErrorMessage = "El campo telefono es obligatorio.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("[0-9]{10,15}$", ErrorMessage = "el campo telefóno no contiene información válida")]
        public string telefono { get; set; }
    
        public string email { get; set; }
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string estado { get; set; }

    }
}