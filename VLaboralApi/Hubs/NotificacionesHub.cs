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
using VLaboralApi.ClasesAuxiliares;

namespace VLaboralApi.Hubs
{
    public class NotificacionesHub : Hub
    {
        private static readonly ConnectionMapping<string> _connections =
      new ConnectionMapping<string>();

        public void EnviarNotificacionPostulacion(NotificacionPostulacion prmNotificacion)
        {
            EnviarNotificacion(prmNotificacion);
        }

        public void EnviarNotificacionExperiencia(NotificacionExperiencia prmNotificacion)
        {
            EnviarNotificacion(prmNotificacion);
        }

        public void EnviarNotificacionPostulanteEtapaAprobada(NotificacionPostulacion prmNotificacion)
        {
            EnviarNotificacion(prmNotificacion);
        }

        private void EnviarNotificacion<T>(T prmNotificacion) where T : Notificacion
        {
            var listadoConexiones = NotificacionesHelper.GetConnectionIds(prmNotificacion.TipoNotificacion.TipoReceptor, prmNotificacion.ReceptorId.ToString());

            foreach (var connectionId in listadoConexiones)
            {
                Clients.Client(connectionId).enviarNotificacion(prmNotificacion);
            }
        }

        public override Task OnConnected()
        {
            var idUsuario = Context.QueryString.Get("userId");
            _connections.Add(idUsuario, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var idUsuario = Context.QueryString.Get("userId");

            _connections.Remove(idUsuario, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            var idUsuario = Context.QueryString.Get("userId");

            if (!_connections.GetConnections(idUsuario).Contains(Context.ConnectionId))
            {
                _connections.Add(idUsuario, Context.ConnectionId);
            }

            return base.OnReconnected();
        }

    }


}

