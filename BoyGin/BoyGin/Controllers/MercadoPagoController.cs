using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BoyGin.AccesoAdatos;
using BoyGin.Models;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;

namespace BoyGin.Controllers
{
    public class MercadoPagoController : Controller
    {
        // GET: MercadoPago
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> MercadoPago()
        {

            // Agrega credenciales
            MercadoPagoConfig.AccessToken = "TEST-1883472601984500-062800-2ee90ae6441b56fe5aa2700bafbae50a-782234269";

            List<Carrito> compras = (List<Carrito>)Session["carrito"];

            double Total = 0;

            if (Session["carrito"] != null)
            {
                compras = (List<Carrito>)Session["carrito"];
                for (int i = 0; i < compras.Count; i++)
                {
   
                        Total += compras[i].cantidadRequerida*compras[i].precioProducto;                 
                }
            }
            decimal TotalApagar = Convert.ToDecimal(Total);

            // Crea el objeto de request de la preference
            var request = new PreferenceRequest
            {
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "http://localhost:64428/Carrito/FinalizaCompra/success",
                    Failure = "http://localhost:64428/Carrito/AgregaCarrito/failure",
                    Pending = "http://localhost:64428/Carrito/AgregaCarrito/pendings",
                },
                AutoReturn = "approved",

                Items = new List<PreferenceItemRequest>
                {

                    new PreferenceItemRequest
                        {

                            Title = "Mi compra",
                            Quantity = 1,
                            CurrencyId = "ARS",
                            UnitPrice = TotalApagar,
                        },
                 },
            };

            // Crea la preferencia usando el client
            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);


            ViewData["IdPreferencia"] = preference.Id;

            return View("MercadoPago");
            //return View (/*"MercadoPago", */preference.Id);
        }

    }
}