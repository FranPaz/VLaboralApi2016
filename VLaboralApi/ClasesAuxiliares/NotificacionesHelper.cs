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

        public NotificacionPostulacion GenerarNotificacionPostulacion(int postulacionId)
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

        public NotificacionExperiencia GenerarNotificacionExperiencia(int experienciaId) //fpaz: devuelve una otificacion  de nueva Experiencia Cargada
        {
            try
            {
                var experiencia = db.ExperienciaLaborals  
                            .Include(prmEmp => prmEmp.Empresa)                  
                            .FirstOrDefault(p => p.Id == experienciaId);


                if (experiencia == null) return null;

                var tipoNotificacion = db.TipoNotificaciones.FirstOrDefault(tn => tn.Valor == "EXP");

                if (tipoNotificacion == null) return null;
                {
                    var notificacion = new NotificacionExperiencia
                    {
                        ExperienciaId = experiencia.Id,
                        // EtapaOfertaId = postulacion.PuestoEtapaOferta.EtapaOfertaId,
                        FechaCreacion = DateTime.Now,
                        FechaPublicacion = DateTime.Now,
                        Mensaje = tipoNotificacion.Mensaje, // "Este mensaje hay que sacarlo de la bd. Por ahora lo hardcodeo aqui",
                        Titulo = tipoNotificacion.Titulo, //"El título lo podemos sacar de la clase directamente o desde la bd. Prefiero desde la bd.",
                        TipoNotificacionId = tipoNotificacion.Id,
                        EmisorId = experiencia.ProfesionalId,
                        ReceptorId = experiencia.Empresa.Id
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

        public IEnumerable<NotificacionPostulacion> GenerarNotificacionesPostulantesPasanEtapa(int ofertaId) 
        {
            try
            {
                var postulaciones = db.Postulacions
                                        .Include(p => p.PuestoEtapaOferta.EtapaOferta.Oferta)
                            .Where(p => p.PuestoEtapaOferta.EtapaOferta.Oferta.Id == ofertaId && p.PuestoEtapaOferta.EtapaOfertaId == p.PuestoEtapaOferta.EtapaOferta.Oferta.IdEtapaActual);
                
                var tipoNotificacion = db.TipoNotificaciones.FirstOrDefault(tn => tn.Valor == "ETAP");

                if (tipoNotificacion == null) return null;
                {
                    var notificaciones = new List<NotificacionPostulacion>();
                    foreach (var postulante in postulaciones)
                    {
                        var notificacion = new NotificacionPostulacion
                        {
                            PostulacionId = postulante.Id,
                            FechaCreacion = DateTime.Now,
                            FechaPublicacion = DateTime.Now,
                            Mensaje = tipoNotificacion.Mensaje, // "Este mensaje hay que sacarlo de la bd. Por ahora lo hardcodeo aqui",
                            Titulo = tipoNotificacion.Titulo, //"El título lo podemos sacar de la clase directamente o desde la bd. Prefiero desde la bd.",
                            TipoNotificacionId = tipoNotificacion.Id,
                            EmisorId = postulante.ProfesionalId,
                            ReceptorId = postulante.PuestoEtapaOferta.EtapaOferta.Oferta.EmpresaId
                        };

                      notificaciones.Add(notificacion);
                    }
                    db.Notificaciones.AddRange(notificaciones);
                    db.SaveChanges();
                    return notificaciones;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<NotificacionInvitacionOferta> GenerarNotificacionesInvitacionesOferta(int ofertaId, List<int> profesionales) //fpaz: devuelve el listado de notificaciones con invitaciones a ofertas privadas
        {
            try
            {
                var oferta = db.Ofertas.Find(ofertaId);

                if (oferta != null) {
                    var tipoNotificacion = db.TipoNotificaciones.FirstOrDefault(tn => tn.Valor == "INV_OFER_PRIV");

                    if (tipoNotificacion != null) {

                        List<NotificacionInvitacionOferta> notificacionesGeneradas = new List<NotificacionInvitacionOferta>();
                        foreach (var profesionalId in profesionales)
                        {
                            var notificacion = new NotificacionInvitacionOferta
                            {
                                OfertaId = oferta.Id,
                                FechaCreacion = DateTime.Now,
                                FechaPublicacion = DateTime.Now,
                                Mensaje = tipoNotificacion.Mensaje, // "Este mensaje hay que sacarlo de la bd. Por ahora lo hardcodeo aqui",
                                Titulo = tipoNotificacion.Titulo, //"El título lo podemos sacar de la clase directamente o desde la bd. Prefiero desde la bd.",
                                TipoNotificacionId = tipoNotificacion.Id,
                                EmisorId = oferta.EmpresaId,
                                ReceptorId = profesionalId
                            };
                            notificacionesGeneradas.Add(notificacion);
                            db.Notificaciones.Add(notificacion);
                        }                        
                        db.SaveChanges();
                        return notificacionesGeneradas;
                    }
                    else
                    {
                        return null;
                    }
                        
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static IEnumerable<string> GetConnectionIds(string tipoReceptor, string receptorId)
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