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
    public class TipoNivelEstudiosController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/TipoNivelEstudios
        public IQueryable<TipoNivelEstudio> GetTipoNivelEstudios()
        {
            return db.TipoNivelEstudios;
        }

        // GET: api/TipoNivelEstudios/5
        [ResponseType(typeof(TipoNivelEstudio))]
        public IHttpActionResult GetTipoNivelEstudio(int id)
        {
            TipoNivelEstudio tipoNivelEstudio = db.TipoNivelEstudios.Find(id);
            if (tipoNivelEstudio == null)
            {
                return NotFound();
            }

            return Ok(tipoNivelEstudio);
        }

        // PUT: api/TipoNivelEstudios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoNivelEstudio(int id, TipoNivelEstudio tipoNivelEstudio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoNivelEstudio.Id)
            {
                return BadRequest();
            }

            db.Entry(tipoNivelEstudio).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoNivelEstudioExists(id))
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

        // POST: api/TipoNivelEstudios
        [ResponseType(typeof(TipoNivelEstudio))]
        public IHttpActionResult PostTipoNivelEstudio(TipoNivelEstudio tipoNivelEstudio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoNivelEstudios.Add(tipoNivelEstudio);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoNivelEstudio.Id }, tipoNivelEstudio);
        }

        // DELETE: api/TipoNivelEstudios/5
        [ResponseType(typeof(TipoNivelEstudio))]
        public IHttpActionResult DeleteTipoNivelEstudio(int id)
        {
            TipoNivelEstudio tipoNivelEstudio = db.TipoNivelEstudios.Find(id);
            if (tipoNivelEstudio == null)
            {
                return NotFound();
            }

            db.TipoNivelEstudios.Remove(tipoNivelEstudio);
            db.SaveChanges();

            return Ok(tipoNivelEstudio);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoNivelEstudioExists(int id)
        {
            return db.TipoNivelEstudios.Count(e => e.Id == id) > 0;
        }
    }
}