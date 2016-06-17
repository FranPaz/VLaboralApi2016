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
    public class TipoDisponibilidadsController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/TipoDisponibilidads
        public IQueryable<TipoDisponibilidad> GetTipoDisponibilidads()
        {
            return db.TipoDisponibilidads;
        }

        // GET: api/TipoDisponibilidads/5
        [ResponseType(typeof(TipoDisponibilidad))]
        public IHttpActionResult GetTipoDisponibilidad(int id)
        {
            TipoDisponibilidad tipoDisponibilidad = db.TipoDisponibilidads.Find(id);
            if (tipoDisponibilidad == null)
            {
                return NotFound();
            }

            return Ok(tipoDisponibilidad);
        }

        // PUT: api/TipoDisponibilidads/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoDisponibilidad(int id, TipoDisponibilidad tipoDisponibilidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoDisponibilidad.Id)
            {
                return BadRequest();
            }

            db.Entry(tipoDisponibilidad).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoDisponibilidadExists(id))
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

        // POST: api/TipoDisponibilidads
        [ResponseType(typeof(TipoDisponibilidad))]
        public IHttpActionResult PostTipoDisponibilidad(TipoDisponibilidad tipoDisponibilidad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoDisponibilidads.Add(tipoDisponibilidad);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoDisponibilidad.Id }, tipoDisponibilidad);
        }

        // DELETE: api/TipoDisponibilidads/5
        [ResponseType(typeof(TipoDisponibilidad))]
        public IHttpActionResult DeleteTipoDisponibilidad(int id)
        {
            TipoDisponibilidad tipoDisponibilidad = db.TipoDisponibilidads.Find(id);
            if (tipoDisponibilidad == null)
            {
                return NotFound();
            }

            db.TipoDisponibilidads.Remove(tipoDisponibilidad);
            db.SaveChanges();

            return Ok(tipoDisponibilidad);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoDisponibilidadExists(int id)
        {
            return db.TipoDisponibilidads.Count(e => e.Id == id) > 0;
        }
    }
}