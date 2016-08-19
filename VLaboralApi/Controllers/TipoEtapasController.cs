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
    public class TipoEtapasController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/TipoEtapas
        public IHttpActionResult GetTiposEtapas()
        {
            try
            {
                var listTiposEtapas = db.TiposEtapas;
                if (listTiposEtapas == null)
                {
                    return BadRequest("No existen Tipos de Etapas Cargados");
                }
                else
                {
                    return Ok(listTiposEtapas);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/TipoEtapas/5
        [ResponseType(typeof(TipoEtapa))]
        public IHttpActionResult GetTipoEtapa(int id)
        {
            TipoEtapa tipoEtapa = db.TiposEtapas.Find(id);
            if (tipoEtapa == null)
            {
                return NotFound();
            }

            return Ok(tipoEtapa);
        }

        // PUT: api/TipoEtapas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoEtapa(int id, TipoEtapa tipoEtapa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoEtapa.Id)
            {
                return BadRequest();
            }

            db.Entry(tipoEtapa).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoEtapaExists(id))
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

        // POST: api/TipoEtapas
        [ResponseType(typeof(TipoEtapa))]
        public IHttpActionResult PostTipoEtapa(TipoEtapa tipoEtapa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TiposEtapas.Add(tipoEtapa);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoEtapa.Id }, tipoEtapa);
        }

        // DELETE: api/TipoEtapas/5
        [ResponseType(typeof(TipoEtapa))]
        public IHttpActionResult DeleteTipoEtapa(int id)
        {
            TipoEtapa tipoEtapa = db.TiposEtapas.Find(id);
            if (tipoEtapa == null)
            {
                return NotFound();
            }

            db.TiposEtapas.Remove(tipoEtapa);
            db.SaveChanges();

            return Ok(tipoEtapa);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoEtapaExists(int id)
        {
            return db.TiposEtapas.Count(e => e.Id == id) > 0;
        }
    }
}