using BoyGin.AccesoAdatos;
using BoyGin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoyGin.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class FacturaController : Controller
    {
        // GET: Factura
        [Required(ErrorMessage = "El campo fecha es obligatorio.")]
        private static string fechaInicio { get; set; }
        [Required(ErrorMessage = "El campo fecha es obligatorio.")]
        private static string fechaFin { get; set; }
        private static List<ReporteFacturacion> objetoPrueba { get; set; }

        private static int incremental { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegistrarFactura()
        {
            GestorBDFactura gestor = new GestorBDFactura();
            VMDetalleFactura factura = new VMDetalleFactura();

            factura.clienteModel = gestor.ListaClientes();
            factura.productoModel = gestor.ListaProducto();

            return View(factura);
        }

        [HttpPost]
        public ActionResult RegistrarFactura(VMDetalleFactura factura)
        {

            GestorBDFactura gestor = new GestorBDFactura();
            //gestor.AgregarFactura(factura.detalleFacturaModel, factura.facturaModel);


            return View("ListadoFacturacion", gestor.ListadoFacturacion());

        }


        public ActionResult ListadoFacturacion()
        {
            GestorBDFactura gestor = new GestorBDFactura();
            List<DTODetalleFactura> listado = gestor.ListadoFacturacion();

            return View(listado);
        }

        public ActionResult Detalles(int idfactura)
        {
            
            GestorBDFactura gestor = new GestorBDFactura();

           

            List<DTODetalleFactura> listado = gestor.ListadoDetalles(idfactura);
            


            return View(listado);

        }

        public ActionResult FacturacionPorFecha(string cadena="", string fi="", string ff="")
        {
            ViewBag.mensaje = "";
            GestorBDFactura gestor = new GestorBDFactura();
            List<DTODetalleFactura> facturacion = null;


            if ( fi != "" && ff != "")
            {
                facturacion = gestor.BuscarFactura(cadena, DateTime.Parse(fi), DateTime.Parse(ff));       
                fechaFin = ff;
                fechaInicio = fi;

            }

            if(( fi == "" || ff == "") && incremental == 1)
            {
                ViewBag.mensaje = "Debe seleccionar el rango de fechas";
            }
            
            ReporteFechas prueba = new ReporteFechas();
            incremental = 1;
            return View(facturacion);

        }

         


        //public ActionResult ReporteFacturacion()
        //{
        //    GestorBDFactura gestor = new GestorBDFactura();
        //    ReporteFacturacion reporte = new ReporteFacturacion();

        //    //Al vicio
        //    //reporte.cantidadPorMes = gestor.ReporteFacturadoPorMes();
        //    reporte.cantidadPorMes = gestor.ReporteCantidadVentasMes();

        //    var ano = DateTime.Today.Year;
        //    var cantidadMeses = 12;

        //    var meses = new List<int>();
        //    var facturadoMes = new List<double>();
        //    for (var mes = 1; mes <= cantidadMeses; mes++)
        //    {
        //        meses.Add(mes);
        //        facturadoMes.Add(0);
        //    }

        //    foreach (var facturado in gestor.ReporteFacturadoPorMes("hola"))
        //    {
        //        facturadoMes[facturado.fecha - 1] = facturado.total;
        //    }

        //    ViewBag.Meses = meses;

        //    //No se usa
        //    ViewBag.Ano = ano;

        //    ViewBag.FacturadoMes = facturadoMes;

        //    var ano2 = DateTime.Today.Year;


        //    var meses2 = new List<int>();
        //    var cantidadVentas = new List<int>();
        //    for (var mes2 = 1; mes2 <= cantidadMeses; mes2++)
        //    {
        //        meses2.Add(mes2);
        //        cantidadVentas.Add(0);
        //    }

        //    foreach (var cantidadVenta in gestor.ReporteCantidadVentasMes())
        //    {
        //        cantidadVentas[cantidadVenta.fecha - 1] = cantidadVenta.cantidad;
        //    }

        //    ViewBag.Meses2 = meses2;
        //    ViewBag.CantidadVentas = cantidadVentas;


        //    return View();
        //}

        public ActionResult ReporteBarras(ReporteFechas reporte)
        {
            GestorBDFactura gestor = new GestorBDFactura();
            List<ReporteFacturacion> reporteFacturacionAño = new List<ReporteFacturacion>();

            string anoInicio = fechaInicio.Substring(0,4);
            string anoFin = fechaFin.Substring(0, 4);
            for (int i = Convert.ToInt32(anoInicio); i < Convert.ToInt32(anoFin)+1; i++)
            {
                reporteFacturacionAño.Add(gestor.ReporteFacturadoPorMes(i.ToString()));
            }
            objetoPrueba = reporteFacturacionAño;
            return View(reporteFacturacionAño);
        }

        [HttpGet]
        public JsonResult prueba()
        {
            return Json(new { result = objetoPrueba }, JsonRequestBehavior.AllowGet);
            
        }

    }
}
