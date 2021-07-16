using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoyGin.Models
{
    public class Proveedor
    {
        public int idProveedor { get; set; }
        [Required(ErrorMessage = "Debe ingresar el nombre/razón social.")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "el campo nombre no contiene información válida")]
        public string nombre_razonSocial { get; set; }
        [Required(ErrorMessage = "Debe ingresar el apellido o tipo de sociedad.")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "el campo apellido no contiene información válida")]
        public string apellido_TipoSoc { get; set; }
        [Required(ErrorMessage = "Debe ingresar el domicilio.")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "el campo domicilio no contiene información válida")]
        public string direccion { get; set; }
        [Required(ErrorMessage = "Debe ingresar el número.")]
        [RegularExpression("[0-9]{1,5}$", ErrorMessage = "el campo número no contiene información válida")]
        public int numeroDireccion { get; set; }

        [Required(ErrorMessage = "Ingrese la localidad.")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "el campo Localidad no contiene información válida")]
        public string localidad { get; set; }
        [Required(ErrorMessage = "Seleccione la provincia.")]
        public int idProvincia { get; set; }
        [Required(ErrorMessage = "El campo correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "el campo Email no es una dirección de correo electrónico válida")]
        public string email { get; set; }
        [Required(ErrorMessage = "Debe ingresar el teléfono.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("[0-9]{10,15}$", ErrorMessage = "el campo telefóno no contiene información válida")]
        public string telefono { get; set; }
        public bool estado { get; set; }
    }
}