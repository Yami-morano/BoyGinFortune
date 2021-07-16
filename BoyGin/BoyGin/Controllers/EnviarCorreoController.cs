using BoyGin.AccesoAdatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BoyGin.AccesoAdatos;


namespace BoyGin.Controllers
{
    [Authorize]
    public class EnviarCorreoController : Controller
    {
        // GET: EnviarCorreo
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public ActionResult EnviarCorreo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnviarCorreo(String asunto, String mensaje, HttpPostedFileBase fichero)
        {
            try
            {
                GestorBDCliente gestorCliente = new GestorBDCliente();
                string currentMail = gestorCliente.getCurrentUser();
                MailMessage correo = new MailMessage();
                correo.From = new MailAddress("boyginfortune@gmail.com");
                correo.To.Add("boyginfortune@gmail.com");
                correo.Subject = asunto;
                correo.Body = "El siguiente mail " + currentMail + " posee la siguiente consulta: " + mensaje;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;

                if (fichero != null)
                {
                    String ruta = Server.MapPath("../Temporal");
                    fichero.SaveAs(ruta + "\\" + fichero.FileName);

                    Attachment adjunto = new Attachment(ruta + "\\" + fichero.FileName);
                    correo.Attachments.Add(adjunto);
                }

                //configuracion del servidor smtp
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 25;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = true;
                string sCuentaCorreo = "boyginfortune@gmail.com";
                string sPasswordCorreo = "Tesis1234";
                smtp.Credentials = new System.Net.NetworkCredential(sCuentaCorreo, sPasswordCorreo);

                smtp.Send(correo);
                ViewBag.Mensaje = "Mensaje enviado correctamente";
            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
            }

            return View();
        }


    }
}