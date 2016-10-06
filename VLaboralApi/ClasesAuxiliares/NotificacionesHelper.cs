using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VlaboralApi.Infrastructure;
using VLaboralApi.Hubs;
using VLaboralApi.Models;
using System.Data;
using System.Data.Entity;

namespace VLaboralApi.ClasesAuxiliares
{
    public class NotificacionesHelper
    {
        private VLaboral_Context db = new VLaboral_Context();

        private static readonly ConnectionMapping<string> _connections =
        new ConnectionMapping<string>();

        public NotificacionPostulacion generarNotificacionPostulacion(int postulacionId)
        {
            try
            {
                var postulacion = db.Postulacions
                                        .Include(p => p.PuestoEtapaOferta.EtapaOferta.Oferta)
                            .FirstOrDefault(p => p.Id == postulacionId);


                if (postulacion == null) return null;

                var tipoNotificacion = db.TipoNotificaciones.FirstOrDefault(tn => tn.Valor == "POS");

                if (tipoNotificacion == null) return null;
                {
                    var notificacion = new NotificacionPostulacion
                    {
                        PostulacionId = postulacion.Id,
                        // EtapaOfertaId = postulacion.PuestoEtapaOferta.EtapaOfertaId,
                        FechaCreacion = DateTime.Now,
                        FechaPublicacion = DateTime.Now,
                        Mensaje = tipoNotificacion.Mensaje, // "Este mensaje hay que sacarlo de la bd. Por ahora lo hardcodeo aqui",
                        Titulo = tipoNotificacion.Titulo, //"El título lo podemos sacar de la clase directamente o desde la bd. Prefiero desde la bd.",
                        TipoNotificacionId = tipoNotificacion.Id,
                        EmisorId = postulacion.ProfesionalId,
                        ReceptorId = postulacion.PuestoEtapaOferta.EtapaOferta.Oferta.EmpresaId
                    };

                    db.Notificaciones.Add(notificacion);
                    db.SaveChanges();
                    return notificacion;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<string> GetConnectionIds(string tipoReceptor, string receptorId)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new VLaboral_Context()));
            foreach (var usuario in manager.Users.Where(u => u.EmailConfirmed).ToList()) //sluna: hay que restringir más el where para obtener una lista más corta.
            {
                var appUsertype = false;
                var usuarioId = false;

                foreach (var claim in manager.GetClaims(usuario.Id))
                {
                    switch (claim.Type)
                    {
                        case "app_usertype":
                            if (claim.Value == tipoReceptor)
                            {
                                appUsertype = true;
                                if (tipoReceptor == "admin")
                                {
                                    usuarioId = true;
                                }
                            }
                            break;

                        case "empresaId":
                            if (claim.Value == receptorId)
                            {
                                usuarioId = true;
                            }
                            break;

                        case "profesionalId":
                            if (claim.Value == receptorId)
                            {
                                usuarioId = true;
                            }
                            break;
                    }
                    if (!usuarioId || !appUsertype) continue; //Si no 
                    return _connections.GetConnections(usuario.Id).ToList();
                }
            }
            return null;
        }
    }
}