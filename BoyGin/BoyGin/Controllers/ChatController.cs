using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoyGin.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Chat()
        {
            return View();
        }
    }
}