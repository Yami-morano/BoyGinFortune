using BoyGin.AccesoAdatos;
using BoyGin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoyGin.Controllers
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegistroProveedor()
        {
            GestorBDProveedor gestor = new GestorBDProveedor();
            VMProveedor proveedor = new VMProveedor();

            proveedor.provinciaModel = gestor.ListadoProvincias();

            return View(proveedor);
        }

        [HttpPost]
        public ActionResult RegistroProveedor(VMProveedor proveedor)
        {

            GestorBDProveedor gestor = new GestorBDProveedor();

            if (ModelState.IsValid == false)
            {
                proveedor.provinciaModel = gestor.ListadoProvincias();
                return View(proveedor);
            }
            gestor.AgregarProveedor(proveedor.proveedorModel);


            return View("ListadoProveedores", gestor.ListadoProveedores());



        }

        public ActionResult ListadoProveedores()
        {
            GestorBDProveedor gestor = new GestorBDProveedor();
            List<DTOProveedor> listado = gestor.ListadoProveedores();

            return View(listado);
        }

        public ActionResult EditarProveedor(int idProveedor)
        {
            GestorBDProveedor gestor = new GestorBDProveedor();
            DTOProveedor resultado = GestorBDProveedor.BuscarProveedor(idProveedor);
            ViewBag.listadoProvincias = gestor.ListadoProvincias();
            ViewBag.Nombre = resultado.nombre_razonSocial;
            return View(resultado);
        }

        [HttpPost]

        public ActionResult EditarProveedor(DTOProveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                bool resultado = GestorBDProveedor.EditarProveedor(proveedor);
                if (resultado)
                {
                    return RedirectToAction("ListadoProveedores", "Proveedor");
                }
                else
                {
                    return View(proveedor);
                }



            }

            return View();
        }

        public ActionResult EliminarProveedor(int idProveedor)
        {
            DTOProveedor resultado = GestorBDProveedor.BuscarProveedor(idProveedor);
            return View(resultado);
        }

        [HttpPost]
        public ActionResult EliminarProveedor(DTOProveedor proveedor)
        {

            if (ModelState.IsValid)
            {
                bool resultado = GestorBDProveedor.EliminarProducto(proveedor);
                if (resultado)
                {
                    return RedirectToAction("ListadoProveedores", "Proveedor");
                }
                else
                {
                    return View(proveedor);
                }
            }

            return View();

        }


    }
}