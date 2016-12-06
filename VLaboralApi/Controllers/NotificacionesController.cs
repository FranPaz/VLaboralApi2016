using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using Antlr.Runtime;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using VlaboralApi.Infrastructure;
using VLaboralApi.ClasesAuxiliares;
using VLaboralApi.Hubs;
using VLaboralApi.Models;
using VLaboralApi.Services;

namespace VLaboralApi.Controllers
{
    //public abstract class ApiControllerWithHub<THub> : ApiController
    //  where THub : IHub
    //{
    //    Lazy<IHubContext> hub = new Lazy<IHubContext>(
    //        () => GlobalHost.ConnectionManager.GetHubContext<THub>()
    //    );

    //    protected IHubContext Hub
    //    {
    //        get { return hub.Value; }
    //    }
    //}

    //public class NotificacionesController : ApiControllerWithHub<NotificacionesHub>

    public class NotificacionesController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/Notificaciones/5
        //[ResponseType(typeof(Notificacion))]
        public IHttpActionResult GetNotificacion(int id, string tipoNotificacion) //fpaz: funcion que devuelve la notificacion y sus objetos asociados, y tambien actualiza la fecha de lectura
        {
            try
            {               

                switch (tipoNotificacion)
                {
                    case "EXP":
                        var notifExp = db.Notificaciones.OfType<NotificacionExperiencia>()
                            .Include(n => n.ExperienciaLaboral.Empresa)
                            .FirstOrDefault(n => n.Id == id);
                        notifExp.FechaLectura = notifExp.FechaLectura ?? DateTime.Now;
                        db.SaveChanges();
                        return Ok(notifExp);
                    case "EXPVER":
                        var notifExpVer = db.Notificaciones.OfType<NotificacionExperiencia>()
                            .Include(n => n.ExperienciaLaboral.Empresa)
                            .FirstOrDefault(n => n.Id == id);
                        notifExpVer.FechaLectura = notifExpVer.FechaLectura ?? DateTime.Now;
                        db.SaveChanges();
                        return Ok(notifExpVer);
                    case "POS":
                        var notifPos = db.Notificaciones.OfType<NotificacionPostulacion>()
                            .Include(n => n.Postulacion.PuestoEtapaOferta.Puesto.Oferta)
                            .Include(p => p.Postulacion.Profesional)
                            .FirstOrDefault(n => n.Id == id);
                        notifPos.FechaLectura = notifPos.FechaLectura ?? DateTime.Now;
                        db.SaveChanges();
                        return Ok(notifPos);
                    case "ETAP":
                        var notifEtap = db.Notificaciones.OfType<NotificacionPostulacion>()
                            .Include(n => n.Postulacion.PuestoEtapaOferta.EtapaOferta.Oferta)
                            .FirstOrDefault(n => n.Id == id);
                        notifEtap.FechaLectura = notifEtap.FechaLectura ?? DateTime.Now;
                        db.SaveChanges();
                        return Ok(notifEtap);
                    case "INV_OFER_PRIV":
                        var notifInvitacion = db.Notificaciones.OfType<NotificacionInvitacionOferta>()
                            .Include(n => n.Oferta)
                            .FirstOrDefault(n => n.Id == id);
                        notifInvitacion.FechaLectura = notifInvitacion.FechaLectura ?? DateTime.Now;
                        db.SaveChanges();
                        return Ok(notifInvitacion);
                    default:
                        return BadRequest("No se han encontrado notificaciones que respondan a los parámetros ingresados.");
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

       

        // GET: api/Notificaciones
        //[Authorize]
        // [ResponseType(typeof(List<Notificacion>))]
        [ResponseType(typeof(CustomPaginateResult<Oferta>))]
        public IHttpActionResult GetNotificacionesRecibidas(int page, int rows)
        {
            //fpaz: comento lo ultimo que hizo santi porque no esta devolviendo resultados que deberia traer

            //var tipoReceptor = Utiles.GetTipoReceptor(User.Identity.GetUserId());
            //if (tipoReceptor == null) return null;

            //var receptorId = Utiles.GetReceptorId(tipoReceptor, User.Identity.GetUserId());
            //if (receptorId == null) return null;

            //var data = Utiles.Paginate(new PaginateQueryParameters(page, rows)
            //    ,db.Notificaciones
            //         .Where(n => n.ReceptorId == receptorId
            //                    && n.FechaPublicacion <= DateTime.Now
            //                    && (n.FechaVencimiento >= DateTime.Now || n.FechaVencimiento == null)
            //                    && n.TipoNotificacion.TipoReceptor == tipoReceptor)
            //         .Include(n => n.TipoNotificacion)
            //    , order => order.OrderByDescending(c => c.FechaPublicacion));
            //return Ok(data);

            var tipoReceptor = Utiles.GetTipoReceptor(User.Identity.GetUserId());
            if (tipoReceptor == null) return null;

            var receptorId = Utiles.GetReceptorId(tipoReceptor, User.Identity.GetUserId());
            if (receptorId == null) return null;

            var totalRows = db.Notificaciones.Count(n => n.ReceptorId == receptorId
                                && n.FechaPublicacion <= DateTime.Now
                                && (n.FechaVencimiento >= DateTime.Now || n.FechaVencimiento == null)
                                && n.TipoNotificacion.TipoReceptor == tipoReceptor);

            var totalPages = (int)Math.Ceiling((double)totalRows / rows);

            var results = db.Notificaciones
                .Where(n => n.ReceptorId == receptorId
                                && n.FechaPublicacion <= DateTime.Now
                                && (n.FechaVencimiento >= DateTime.Now || n.FechaVencimiento == null)
                                && n.TipoNotificacion.TipoReceptor == tipoReceptor)
                                .Include(n => n.TipoNotificacion)
                                .OrderByDescending(n => n.FechaPublicacion)
                .Skip((page - 1) * rows) //SLuna: -1 Para manejar indice(1) en pagina
                .Take(rows)
                .ToList();

            var result = new CustomPaginateResult<Notificacion>()
            {
                PageSize = rows,
                TotalRows = totalRows,
                TotalPages = totalPages,
                CurrentPage = page,
                Results = results
            };

            return Ok(result);
        }

        

        [ResponseType(typeof(void))]
        public IHttpActionResult PutNotificacion(int id, Notificacion notificacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notificacion.Id)
            {
                return BadRequest();
            }

            notificacion.FechaLectura = DateTime.Now;
            db.Entry(notificacion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        private void SendNotificacion(string who, Notificacion notificacion)
        {



        }

        // POST: api/Notificaciones
        [ResponseType(typeof(Notificacion))]
        public IHttpActionResult PostNotificacionEtapaAprobada(int postulacionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var postulacion = db.Postulacions
                                    .Include(p => p.PuestoEtapaOferta.EtapaOferta.Oferta)
                        .FirstOrDefault(p => p.Id == postulacionId);


            if (postulacion == null) return BadRequest(ModelState);

            var tipoNotificacion = db.TipoNotificaciones.FirstOrDefault(tn => tn.Valor == "ETAP");

            if (tipoNotificacion == null) return BadRequest(ModelState);
            {
                var notificacion = new NotificacionPostulacion()
                {
                    PostulacionId = postulacion.Id,
                    // EtapaOfertaId = postulacion.PuestoEtapaOferta.EtapaOfertaId,
                    FechaCreacion = DateTime.Now,
                    FechaPublicacion = DateTime.Now,
                    Mensaje = tipoNotificacion.Mensaje, // "Este mensaje hay que sacarlo de la bd. Por ahora lo hardcodeo aqui",
                    Titulo = tipoNotificacion.Titulo, //"El título lo podemos sacar de la clase directamente o desde la bd. Prefiero desde la bd.",
                    TipoNotificacionId = tipoNotificacion.Id,
                    ReceptorId = postulacion.ProfesionalId,
                    EmisorId = postulacion.PuestoEtapaOferta.EtapaOferta.Oferta.EmpresaId
                };

                db.Notificaciones.Add(notificacion);
                db.SaveChanges();

                return Ok();
                //  return CreatedAtRoute("DefaultApi", new { id = notificacion.Id }, notificacion);
            }
        }

        // POST: api/Notificaciones
        [ResponseType(typeof(Notificacion))]
        public IHttpActionResult PostNotificacion(Notificacion notificacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            //Existe Emisor

            //Existe Receptor

            //Existe tipo



            db.Notificaciones.Add(notificacion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = notificacion.Id }, notificacion);
        }

        // DELETE: api/Notificaciones/5
        [ResponseType(typeof(Notificacion))]
        public IHttpActionResult DeleteNotificacion(int id)
        {
            Notificacion notificacion = db.Notificaciones.Find(id);
            if (notificacion == null)
            {
                return NotFound();
            }

            db.Notificaciones.Remove(notificacion);
            db.SaveChanges();

            return Ok(notificacion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotificacionExists(int id)
        {
            return db.Notificaciones.Count(e => e.Id == id) > 0;
        }
    }


}