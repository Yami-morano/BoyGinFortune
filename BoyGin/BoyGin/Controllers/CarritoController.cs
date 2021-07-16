using BoyGin.AccesoAdatos;
using BoyGin.Models;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MercadoPago.Resource.Preference;

namespace BoyGin.Controllers
{
    [Authorize]
    public class CarritoController : Controller
    {
        private static string idDetalleFactura { get; set; }
        //GET: Carrito
        public ActionResult Index()
        {
            return View();
        }

        private GestorBDFactura BD = new GestorBDFactura();

        [HttpPost]
        public JsonResult AgregaCarrito(int id, int cantidad)
        {
            int cantidadTotal = cantidad;
            int indexExistente = -1;
            List<Carrito> compras = new List<Carrito>();

            if (Session["carrito"] != null)
            {
                compras = (List<Carrito>)Session["carrito"];
                for (int i = 0; i < compras.Count; i++)
                {
                    if (compras[i].idProducto == id)
                    {
                        indexExistente = i;
                        cantidadTotal += compras[i].cantidadRequerida;
                    }
                }
            }

            int stock = Convert.ToInt32(BD.validadorStock(id));
            if (stock < cantidadTotal)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

            if (indexExistente != -1)
            {
                compras[indexExistente].cantidadRequerida += cantidad;
            }
            else
            {
                compras.Add((BD.CarritoProducto(id, cantidad)));
            }
            Session["carrito"] = compras;


            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AgregaCarrito()
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
           List<Carrito> compras = (List <Carrito>)Session["carrito"];
            compras.RemoveAt(getIndex(id));
            return View("AgregaCarrito");
        }


        public ActionResult FinalizaCompra()
        {
            GestorBDFactura gestorFactura = new GestorBDFactura();
            List<Carrito> compras = (List<Carrito>)Session["carrito"];
            if (compras.Count == 0)
            {
                GestorBD gestorProducto = new GestorBD();
                List<DTOProducto> listadoProducto = gestorProducto.ListadoProducto();
                return View("ProductoCarrito", listadoProducto);
            }
            string idFactura = gestorFactura.agregarFactura();
            
            for (int i = 0; i < compras.Count; i++)
            {
                DetalleFactura detalle = new DetalleFactura(compras[i].idProducto, compras[i].precioProducto, compras[i].cantidadRequerida);
                gestorFactura.AgregarDetalleFactura(detalle, idFactura);
            }
            
            
            ////ACA VA EL ARMADO DE FACTURA
            idDetalleFactura = idFactura;

            GestorBDFactura gestor = new GestorBDFactura();
            List<DTODetalleFactura> listado = gestor.Recibo(idFactura);
            ViewBag.flag = false;

            

            compras = null;
            Session["carrito"] = compras;
            return View(listado);
        }
        private List<DTODetalleFactura> getDetalle()
        {
            GestorBDFactura gestor = new GestorBDFactura();
            List<DTODetalleFactura> listado = gestor.Recibo(idDetalleFactura);
            return listado;
        }
        private int getIndex(int id)
        {
            List<Carrito> compras = (List<Carrito>)Session["carrito"];
            for (int i = 0; i < compras.Count; i++)
            {   
                if (compras[i].idProducto == id)
                    return i;
            }

            return -1;
        }

        public ActionResult print()
        {
            ViewBag.flag = true;
            
            return new Rotativa.ViewAsPdf("FinalizaCompra", getDetalle());

        }

        public ActionResult soloParaVer()
        {

            return View();

        }

       

    }
}