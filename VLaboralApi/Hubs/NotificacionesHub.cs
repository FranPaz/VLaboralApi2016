using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Owin;
using VLaboralApi.Controllers;
using VLaboralApi.Models;

namespace VLaboralApi.Hubs
{
    public class NotificacionesHub : Hub
    {
        public void Enviar(int  NotificacionId)
        {
            //var notificaciones = D
            Clients.All.actualizarNotificaciones(NotificacionId);


        }

        private static readonly ConnectionMapping<string> _connections =
         new ConnectionMapping<string>();
   

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }
     
        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }
       
        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}

''