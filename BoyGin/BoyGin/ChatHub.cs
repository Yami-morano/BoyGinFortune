﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace BoyGin
{
    public class ChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
        public void enviar(string nombre, string mensaje)
        {
            Clients.All.enviarmensaje(nombre, mensaje);
        }
    }
}