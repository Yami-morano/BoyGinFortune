using BoyGin.AccesoAdatos;
using BoyGin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoyGin.Controllers
{
    [Authorize(Roles="Administrador")]
    public class MateriaPrimaController : Controller
    {
        // GET: MateriaPrima
        public ActionResult Principal()
        {
            return View();
        }

        public ActionResult CompraMateriaPrima()
        {
            GestorBD gestor = new GestorBD();
            VMCompraMateriaPrima mp = new VMCompraMateriaPrima();

            mp.materiasPrimas = gestor.ComboMateriaPrima();
            mp.proveedorModel = gestor.ListadoProveedores();

            return View(mp);
        }

       
        [HttpPost]
        public ActionResult CompraMateriaPrima(VMCompraMateriaPrima compra)
        {

            GestorBD gestor = new GestorBD();

            if (ModelState.IsValid == false)
            {
                compra.materiasPrimas = gestor.ComboMateriaPrima();
                compra.proveedorModel = gestor.ListadoProveedores();
                return View(compra);
            }

            gestor.ComprarMateriaPrima(compra.compraMPModel);


            return View("MateriasPrimasStock", gestor.ListadoMateriasPrimas());



        }
       
        public ActionResult MateriasPrimasStock()
        {
            GestorBD gestor = new GestorBD();
            List<DTOMateriaPrima> listado = gestor.ListadoMateriasPrimas();

            return View(listado);
        }

       
        public ActionResult ListaMateriasPrimas()
        {
            GestorBD gestor = new GestorBD();
            List<MateriaPrima> listado = gestor.ListaMateriaPrima();

            return View(listado);
        }

       
        public ActionResult AltaMateriaPrima()
        {
            GestorBD gestor = new GestorBD();
            VMMateriaPrima mp = new VMMateriaPrima();

            return View(mp);
        }

      
        [HttpPost]
        public ActionResult AltaMateriaPrima (VMMateriaPrima materiaPrima)
        {

            GestorBD gestor = new GestorBD();
            if (ModelState.IsValid == false)
            {
                return View(materiaPrima);
            }
            gestor.AgregarMateriaPrima(materiaPrima.materiaPrimaModel);

            return View("ListaMateriasPrimas", gestor.ListaMateriaPrima());

        }
       
        public ActionResult EditarMateriaPrima(int idMateriaPrima)
        {

            MateriaPrima resultado = GestorBD.BuscarMateriaPrima(idMateriaPrima);
            ViewBag.DescripcionMateriaPrima = resultado.descripcionMateriaPrima;

            return View(resultado);
        }


       
        [HttpPost]

        public ActionResult EditarMateriaPrima(MateriaPrima materiaPrima)
        {
            if (ModelState.IsValid)
            {
                bool resultado = GestorBD.EditarMateriaPrima(materiaPrima);
                if (resultado)
                {
                    return RedirectToAction("ListaMateriasPrimas", "MateriaPrima");
                }
                else
                {
                    return View(materiaPrima);
                }



            }

            return View();
        }
       
        public ActionResult EliminarMateriaPrima(int idMateriaPrima)
        {
            MateriaPrima resultado = GestorBD.BuscarMateriaPrima(idMateriaPrima);
            return View(resultado);
        }

        
        [HttpPost]
        public ActionResult EliminarMateriaPrima(MateriaPrima materiaPrima)
        {

            if (ModelState.IsValid)
            {
                bool resultado = GestorBD.EliminaMateriaPrima(materiaPrima);
                if (resultado)
                {
                    return RedirectToAction("ListaMateriasPrimas", "MateriaPrima");
                }
                else
                {
                    return View(materiaPrima);
                }
            }

            return View();

        }



    }
}