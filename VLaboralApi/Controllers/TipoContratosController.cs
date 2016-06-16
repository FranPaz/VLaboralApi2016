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
    public class TipoContratosController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/TipoContratos
        public IQueryable<TipoContrato> GetTipoContratoes()
        {
            return db.TipoContratoes;
        }

        // GET: api/TipoContratos/5
        [ResponseType(typeof(TipoContrato))]
        public IHttpActionResult GetTipoContrato(int id)
        {
            TipoContrato tipoContrato = db.TipoContratoes.Find(id);
            if (tipoContrato == null)
            {
                return NotFound();
            }

            return Ok(tipoContrato);
        }

        // PUT: api/TipoContratos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoContrato(int id, TipoContrato tipoContrato)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoContrato.Id)
            {
                return BadRequest();
            }

            db.Entry(tipoContrato).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoContratoExists(id))
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

        // POST: api/TipoContratos
        [ResponseType(typeof(TipoContrato))]
        public IHttpActionResult PostTipoContrato(TipoContrato tipoContrato)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoContratoes.Add(tipoContrato);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoContrato.Id }, tipoContrato);
        }

        // DELETE: api/TipoContratos/5
        [ResponseType(typeof(TipoContrato))]
        public IHttpActionResult DeleteTipoContrato(int id)
        {
            TipoContrato tipoContrato = db.TipoContratoes.Find(id);
            if (tipoContrato == null)
            {
                return NotFound();
            }

            db.TipoContratoes.Remove(tipoContrato);
            db.SaveChanges();

            return Ok(tipoContrato);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoContratoExists(int id)
        {
            return db.TipoContratoes.Count(e => e.Id == id) > 0;
        }
    }
}