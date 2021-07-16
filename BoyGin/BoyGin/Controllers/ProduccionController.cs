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


        //public ActionResult ReporteporAño()
        //{
        //    GestorBDProduccion gestor = new GestorBDProduccion();

        //    ReporteProduccion listaAños= new ReporteProduccion();

        //    listaAños.añoCombo = gestor.años();

        //    return View(listaAños);

        //}

        //[HttpPost]
        //public ActionResult ReporteporAño(ReporteProduccion model)
        //{

        //    int año = Convert.ToInt32(Request.Form[0]);


        //    GestorBDProduccion gestor = new GestorBDProduccion();
        //    ReporteProduccion listaAños = new ReporteProduccion();
        //    listaAños.cantidadProducida=gestor.
        //    listaAños.añoCombo = gestor.años();
        //    ListaTipo.ListaProducto = gestor.ListadoPorTipoProducto(idTipoProducto);


        //    var listaNombreProducto = new List<string>();
        //    var listaStockProducto = new List<double>();
        //    for (int i = 0; i < ListaTipo.ListaProducto.Count; i++)
        //    {
        //        listaNombreProducto.Add(ListaTipo.ListaProducto[i].nombreProducto);
        //        listaStockProducto.Add(ListaTipo.ListaProducto[i].stockProducto);
        //    }

        //    ViewBag.listaNomProd = listaNombreProducto;
        //    ViewBag.listaStockProd = listaStockProducto;

        //    return View(ListaTipo);
        //}

        //[HttpGet]
        //public JsonResult graficoAño()
        //{
        //    GestorBDProduccion gestor = new GestorBDProduccion();
        //    return Json(new { result = gestor.ReporteProduccion() }, JsonRequestBehavior.AllowGet);
        //}



    }
}
