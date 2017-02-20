using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
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

        public void EnviarNotificacionInvitacionOfertaPriv(List<NotificacionInvitacionOferta> prmNotificaciones) //fpaz: envia una notificacion de invitacion a oferta privada
        {
            if (prmNotificaciones != null)
                foreach (var item in prmNotificaciones)
                {
                    EnviarNotificacion(item);
                }
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

