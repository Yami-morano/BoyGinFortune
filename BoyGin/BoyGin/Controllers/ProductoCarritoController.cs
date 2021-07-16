using BoyGin.AccesoAdatos;
using BoyGin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoyGin.Controllers
{
    public class ProductoCarritoController : Controller
    {
        [Authorize]
        public ActionResult ProductoCarrito()
        {
            GestorBD gestor = new GestorBD();
            List<DTOProducto> listado = gestor.ListadoProducto();
            
            return View(listado);
        }

    }
}