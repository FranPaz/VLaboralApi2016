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
    public class TiposNotificacionesController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/TiposNotificaciones
        public IQueryable<TipoNotificacion> GetTipoNotificaciones()
        {
            return db.TipoNotificaciones;
        }

        // GET: api/TiposNotificaciones/5
        [ResponseType(typeof(TipoNotificacion))]
        public IHttpActionResult GetTipoNotificacion(string prmTipoReceptor) //fpaz: devuelve los tipos de notificaciones de un receptor
        {
            try
            {
                var listTiposNotif = (from t in db.TipoNotificaciones
                                      where t.TipoReceptor == prmTipoReceptor
                                      select t);
                if (listTiposNotif == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(listTiposNotif);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/TiposNotificaciones/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoNotificacion(int id, TipoNotificacion tipoNotificacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoNotificacion.Id)
            {
                return BadRequest();
            }

            db.Entry(tipoNotificacion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoNotificacionExists(id))
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

        // POST: api/TiposNotificaciones
        [ResponseType(typeof(TipoNotificacion))]
        public IHttpActionResult PostTipoNotificacion(TipoNotificacion tipoNotificacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoNotificaciones.Add(tipoNotificacion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoNotificacion.Id }, tipoNotificacion);
        }

        // DELETE: api/TiposNotificaciones/5
        [ResponseType(typeof(TipoNotificacion))]
        public IHttpActionResult DeleteTipoNotificacion(int id)
        {
            TipoNotificacion tipoNotificacion = db.TipoNotificaciones.Find(id);
            if (tipoNotificacion == null)
            {
                return NotFound();
            }

            db.TipoNotificaciones.Remove(tipoNotificacion);
            db.SaveChanges();

            return Ok(tipoNotificacion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoNotificacionExists(int id)
        {
            return db.TipoNotificaciones.Count(e => e.Id == id) > 0;
        }
    }
}