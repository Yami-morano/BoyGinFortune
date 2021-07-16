using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoyGin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Terminos()
        {
            

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult TerminosYcondiciones()
        {
            

            return View();
        }

        public ActionResult PoliticasDePrivacidad()
        {


            return View();
        }

        public ActionResult PreguntasFrecuentes()
        {


            return View();
        }
    }
}