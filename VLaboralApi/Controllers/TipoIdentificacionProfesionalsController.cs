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
    public class TipoIdentificacionProfesionalsController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/TipoIdentificacionProfesionals
        public IQueryable<TipoIdentificacionProfesional> GetTiposIdentificacionesProfesionales()
        {
            return db.TiposIdentificacionesProfesionales;
        }

        // GET: api/TipoIdentificacionProfesionals/5
        [ResponseType(typeof(TipoIdentificacionProfesional))]
        public IHttpActionResult GetTipoIdentificacionProfesional(int id)
        {
            TipoIdentificacionProfesional tipoIdentificacionProfesional = db.TiposIdentificacionesProfesionales.Find(id);
            if (tipoIdentificacionProfesional == null)
            {
                return NotFound();
            }

            return Ok(tipoIdentificacionProfesional);
        }

        // PUT: api/TipoIdentificacionProfesionals/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoIdentificacionProfesional(int id, TipoIdentificacionProfesional tipoIdentificacionProfesional)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoIdentificacionProfesional.Id)
            {
                return BadRequest();
            }

            db.Entry(tipoIdentificacionProfesional).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoIdentificacionProfesionalExists(id))
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

        // POST: api/TipoIdentificacionProfesionals
        [ResponseType(typeof(TipoIdentificacionProfesional))]
        public IHttpActionResult PostTipoIdentificacionProfesional(TipoIdentificacionProfesional tipoIdentificacionProfesional)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TiposIdentificacionesProfesionales.Add(tipoIdentificacionProfesional);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoIdentificacionProfesional.Id }, tipoIdentificacionProfesional);
        }

        // DELETE: api/TipoIdentificacionProfesionals/5
        [ResponseType(typeof(TipoIdentificacionProfesional))]
        public IHttpActionResult DeleteTipoIdentificacionProfesional(int id)
        {
            TipoIdentificacionProfesional tipoIdentificacionProfesional = db.TiposIdentificacionesProfesionales.Find(id);
            if (tipoIdentificacionProfesional == null)
            {
                return NotFound();
            }

            db.TiposIdentificacionesProfesionales.Remove(tipoIdentificacionProfesional);
            db.SaveChanges();

            return Ok(tipoIdentificacionProfesional);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoIdentificacionProfesionalExists(int id)
        {
            return db.TiposIdentificacionesProfesionales.Count(e => e.Id == id) > 0;
        }
    }
}