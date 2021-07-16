using BoyGin.AccesoAdatos;
using BoyGin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoyGin.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult RegistroCliente()
        {
            GestorBDCliente gestor = new GestorBDCliente();
            VMCliente cliente = new VMCliente();

            cliente.provinciaModel = gestor.ListadoProvincias();
          

            return View(cliente);
        }

        [HttpPost]
        public ActionResult RegistroCliente(VMCliente cliente)
        {


            GestorBDCliente gestor = new GestorBDCliente();
            if (ModelState.IsValid == false)
            {
                cliente.provinciaModel = gestor.ListadoProvincias();
                return View(cliente);
            }
            gestor.AgregarCliente(cliente.clienteModel);
          

            return RedirectToAction("Index","Home");
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult ListadoClientes()
        {
            GestorBDCliente gestor = new GestorBDCliente();
            List<DTOCliente> listado = gestor.ListadoClientes();

            return View(listado);
        }

        public ActionResult EditarCliente(int idCliente)
        {
            GestorBDProveedor gestorProveedor = new GestorBDProveedor();
            DTOCliente resultado = GestorBDCliente.BuscarCliente(idCliente);
            ViewBag.Nombre = resultado.nombreCliente;
            ViewBag.listadoProvincias = gestorProveedor.ListadoProvincias();
            return View(resultado);
        }



        [HttpPost]

        public ActionResult EditarCliente(DTOCliente cliente)
        {
            if (ModelState.IsValid)
            {
                bool resultado = GestorBDCliente.EditarCliente(cliente);
                if (resultado)
                {
                    return RedirectToAction("ListadoClientes", "Cliente");
                }
                else
                {
                    return View(cliente);
                }



            }

            return View();
        }

        public ActionResult EliminarCliente(int idCLiente)
        {
            DTOCliente resultado = GestorBDCliente.BuscarCliente(idCLiente);
            return View(resultado);
        }

        [HttpPost]
        public ActionResult EliminarCliente(DTOCliente cliente, int idCliente)
        {

            if (ModelState.IsValid)
            {
                bool resultado = GestorBDCliente.EliminarCliente(cliente, idCliente);
                if (resultado)
                {
                    return RedirectToAction("ListadoClientes", "Cliente");
                }
                else
                {
                    return View(cliente);
                }
            }

            return View();

        }


        public ActionResult BuscarCliente(string nombre = "", string apellido = "")
        {
            GestorBDCliente gestor = new GestorBDCliente();

            List<DTOCliente> buscarCliente = null;
            if (nombre != "" || apellido != "")
            {
                buscarCliente = gestor.BuscarClienteFiltro(nombre, apellido);
            }


            return View(buscarCliente);

        }
    }
}
