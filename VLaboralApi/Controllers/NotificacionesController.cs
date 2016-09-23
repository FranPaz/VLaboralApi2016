using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VLaboralApi.Models;

namespace VLaboralApi.Controllers
{
    public class NotificacionesController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/Notificaciones
        public IQueryable<Notificacion> GetNotificaciones()
        {
            return db.Notificaciones;
        }

        // GET: api/Notificaciones
        public IHttpActionResult GetNotificaciones(string UsuarioId)
        {
            var resultado = db.Notificaciones
                .Where(n => n.ReceptorId == UsuarioId)
                .Include(n => n.TipoNotificacion);
               
            return Ok(resultado);
        }


        // GET: api/Notificaciones/5
        [ResponseType(typeof(Notificacion))]
        public IHttpActionResult GetNotificacion(int id)
        {
            Notificacion notificacion = db.Notificaciones.Find(id);
            if (notificacion == null)
            {
                return NotFound();
            }

            return Ok(notificacion);
        }

        // PUT: api/Notificaciones/5
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

        // POST: api/Notificaciones
        [ResponseType(typeof(Notificacion))]
        public IHttpActionResult PostNotificacion(Notificacion notificacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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