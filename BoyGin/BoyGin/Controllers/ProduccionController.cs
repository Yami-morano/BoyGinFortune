using BoyGin.AccesoAdatos;
using BoyGin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoyGin.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProduccionController : Controller
    {
        // GET: Producion
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult RegistrarProduccion()
        {
            GestorBDProduccion gestor = new GestorBDProduccion();
            VMProduccion produccion = new VMProduccion();

            produccion.materiaPrimaModel = gestor.ListaMateriaPrima();
            produccion.productoModel = gestor.ListaProducto();

            return View(produccion);

        }


        [HttpGet]
        public JsonResult stockController(int id, int cantidad)
        {
            GestorBDProduccion gestor = new GestorBDProduccion();
            int stock = Convert.ToInt32(gestor.verificadorStock(id));
            if (stock < cantidad)
            {
                return Json(new { result = true, cant = stock }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult RegistrarProduccion(VMProduccion produccion)
        {
            GestorBDProduccion gestor = new GestorBDProduccion();

            if (ModelState.IsValid == false)
            {
                produccion.materiaPrimaModel = gestor.ListaMateriaPrima();
                produccion.productoModel = gestor.ListaProducto();
                return View(produccion);
            }

            string idProduccion = gestor.AgregarHistorial(produccion.historialProduModel);

            foreach (var item in produccion.conceptos)
            {
                gestor.AgregarProduccion(produccion.produccionmodel, produccion.historialProduModel, Convert.ToInt32(item.materiaPrimaId), Convert.ToInt32(item.cantidadMateriaPrimaUsada), idProduccion);
            }

            gestor.ActualizarStockProducto(produccion.produccionmodel.idProducto, produccion.historialProduModel.cantidadProducida);


            return View("ListadoProduccion", gestor.ListadoProduccion());

        }


        public ActionResult ListadoProduccion()
        {
            GestorBDProduccion gestor = new GestorBDProduccion();
            List<DTOProduccion_MateriasPrimas> listado = gestor.ListadoProduccion();

            return View(listado);
        }

        public ActionResult DetalleProduccion(int idProduccion)
        {

            GestorBDProduccion gestor = new GestorBDProduccion();

            List<DTOProduccion_MateriasPrimas> listado = gestor.ListadoDetallesProduccion(idProduccion);

            return View(listado);

        }

        //public ActionResult ReporteProduccion()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public JsonResult grafico()
        //{
        //    GestorBDProduccion gestor = new GestorBDProduccion();
        //    return Json(new { result = gestor.ReporteProduccion() }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult ReporteMateriasPrimas()
        {
            return View();
        }

        [HttpGet]
        public JsonResult graficoMateriaPrima()
        {
            GestorBDProduccion gestor = new GestorBDProduccion();
            return Json(new { result = gestor.ReporteMateriasPrimas() }, JsonRequestBehavior.AllowGet);
        }


      
        public ActionResult ReporteporAño(ReporteProduccion model)
        {

            GestorBDProduccion gestor = new GestorBDProduccion();
            ReporteProduccion listaAños = new ReporteProduccion();
           
            listaAños.añoCombo = gestor.años();

            listaAños.cantidadProducida = gestor.cantidadproducidaAño();


            var listaAno = new List<int>();
            var listaCantidad = new List<int>();
            for (int i = 0; i < listaAños.cantidadProducida.Count; i++)
            {
                listaAno.Add(listaAños.cantidadProducida[i].ano);
                listaCantidad.Add(listaAños.cantidadProducida[i].cantidad);
            }

            ViewBag.listaAno = listaAno;
            ViewBag.listaCantidad = listaCantidad;

            return View(listaAños);
        }

        //[HttpGet]
        //public JsonResult graficoAño()
        //{
        //    GestorBDProduccion gestor = new GestorBDProduccion();
        //    return Json(new { result = gestor.ReporteProduccion() }, JsonRequestBehavior.AllowGet);
        //}



    }
}
