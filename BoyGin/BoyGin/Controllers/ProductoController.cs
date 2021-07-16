using BoyGin.AccesoAdatos;
using BoyGin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoyGin.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        // GET: Producto
        public ActionResult Principal()
        {
            return View();
        }



        public ActionResult Alta()
        {
            GestorBD gestor = new GestorBD();
            VMProducto vi = new VMProducto();

            vi.tipos = gestor.ListadoTipo();

            return View(vi);
        }

        [HttpPost]
        public ActionResult Alta(VMProducto producto)
        {

            GestorBD gestor = new GestorBD();
            if (ModelState.IsValid == false)
            {
                producto.tipos = gestor.ListadoTipo();
                return View(producto);
            }

                gestor.AgregarProducto(producto.productoModel);


            return View("Listado", gestor.ListadoProducto());

            

        }

        public ActionResult Listado()
        {
            GestorBD gestor = new GestorBD();
            List<DTOProducto> listado = gestor.ListadoProducto();

            return View(listado);
        }

        public ActionResult Editar(int idProducto)
        {
            GestorBD gestor = new GestorBD();
            DTOProducto resultado = GestorBD.BuscarProducto(idProducto);
            ViewBag.Nombre = resultado.nombreProducto;
            ViewBag.ListaTipoProducto = gestor.ListadoTipo();
            return View(resultado);
        }



        [HttpPost]

        public ActionResult Editar(DTOProducto producto)
        {
            if (ModelState.IsValid)
            {
                bool resultado = GestorBD.EditarProducto(producto);
                if (resultado)
                {
                    return RedirectToAction("Listado", "Producto");
                }
                else
                {
                    return View(producto);
                }



            }

            return View();
        }

        public ActionResult Eliminar(int idProducto)
        {
            DTOProducto resultado = GestorBD.BuscarProducto(idProducto);
            return View(resultado);
        }

        [HttpPost]
        public ActionResult Eliminar(DTOProducto model)
        {

            if (ModelState.IsValid)
            {
                bool resultado = GestorBD.EliminarProducto(model);
                if (resultado)
                {
                    return RedirectToAction("Listado", "Producto");
                }
                else
                {
                    return View(model);
                }
            }

            return View();

        }


        public ActionResult ListadoPorTipoProducto()
        {
            GestorBD gestor = new GestorBD();
            ListaPorTipoProducto ListaTipo = new ListaPorTipoProducto();

            ListaTipo.tiposLista = gestor.ListadoTipo();

            return View(ListaTipo);
        }

        [HttpPost]
        public ActionResult ListadoPorTipoProducto(ListaPorTipoProducto model)
        {
          

            int idTipoProducto = Convert.ToInt32(Request.Form[0]);
                
                
            GestorBD gestor = new GestorBD();
            ListaPorTipoProducto ListaTipo = new ListaPorTipoProducto();
            ListaTipo.ListaProducto = gestor.ListadoPorTipoProducto(idTipoProducto);
            ListaTipo.tiposLista = gestor.ListadoTipo();

            //var listaNombreProducto = new List<string>();
            //var listaStockProducto = new List<double>();
            //for (int i = 0; i < ListaTipo.ListaProducto.Count; i++)
            //{
            //    listaNombreProducto.Add(ListaTipo.ListaProducto[i].nombreProducto);
            //    listaStockProducto.Add(ListaTipo.ListaProducto[i].stockProducto);
            //}

            //ViewBag.listaNomProd = listaNombreProducto;
            //ViewBag.listaStockProd = listaStockProducto;


            ListaTipo.cantidadTipo = gestor.CantidadPorTipoProducto();

            var listaNombreTipo = new List<string>();
            var listaStockProducto = new List<double>();

            for (int i = 0; i < ListaTipo.cantidadTipo.Count; i++)
            {
                listaNombreTipo.Add(ListaTipo.cantidadTipo[i].nombreTipo);
                listaStockProducto.Add(ListaTipo.cantidadTipo[i].stockProducto);
            }

            ViewBag.listaNomProd = listaNombreTipo;
            ViewBag.listaStockProd = listaStockProducto;



            return View(ListaTipo);
        }



        public ActionResult ReporteProductosVendidos()
        {
            GestorBD gestor = new GestorBD();

            int cantidad = gestor.ReporteCantidadTotal();

            ViewBag.CantidadTotal = cantidad;

            gestor.ReporteProductosVendidos();

            return View();
        }
        [HttpGet]
        public JsonResult grafico()
        {
            GestorBD gestor = new GestorBD();
            List<ReporteProducto> reporte = gestor.ReporteProductosVendidos();
            return Json(new { result = reporte}, JsonRequestBehavior.AllowGet);   
        }


       
        [HttpGet]
        public JsonResult graficoStock()
        {
            GestorBD gestor = new GestorBD();
            return Json(new { result = gestor.StockProducto() }, JsonRequestBehavior.AllowGet);
        }

    }


}
