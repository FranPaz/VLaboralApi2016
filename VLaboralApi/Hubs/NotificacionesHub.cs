using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            string idUsuario = Context.QueryString.Get("access_token");
            _connections.Add(idUsuario, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string idUsuario = Context.QueryString.Get("access_token");

            _connections.Remove(idUsuario, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string idUsuario = Context.QueryString.Get("access_token");

            if (!_connections.GetConnections(idUsuario).Contains(Context.ConnectionId))
            {
                _connections.Add(idUsuario, Context.ConnectionId);
            }

            return base.OnReconnected();
        }

    }


}

