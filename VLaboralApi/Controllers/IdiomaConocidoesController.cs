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
    public class IdiomaConocidoesController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/IdiomaConocidoes
        public IQueryable<IdiomaConocido> GetIdiomaConocidoes()
        {
            return db.IdiomaConocidoes;
        }

        // GET: api/IdiomaConocidoes/5
        [ResponseType(typeof(IdiomaConocido))]
        public IHttpActionResult GetIdiomaConocido(int id)
        {
            IdiomaConocido idiomaConocido = db.IdiomaConocidoes.Find(id);
            if (idiomaConocido == null)
            {
                return NotFound();
            }

            return Ok(idiomaConocido);
        }

        // PUT: api/IdiomaConocidoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutIdiomaConocido(int id, IdiomaConocido idiomaConocido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != idiomaConocido.Id)
            {
                return BadRequest();
            }

            db.Entry(idiomaConocido).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdiomaConocidoExists(id))
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

        // POST: api/IdiomaConocidoes
        [ResponseType(typeof(IdiomaConocido))]
        public IHttpActionResult PostIdiomaConocido(IdiomaConocido idiomaConocido)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.IdiomaConocidoes.Add(idiomaConocido);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = idiomaConocido.Id }, idiomaConocido);
        }

        // DELETE: api/IdiomaConocidoes/5
        [ResponseType(typeof(IdiomaConocido))]
        public IHttpActionResult DeleteIdiomaConocido(int id)
        {
            IdiomaConocido idiomaConocido = db.IdiomaConocidoes.Find(id);
            if (idiomaConocido == null)
            {
                return NotFound();
            }

            db.IdiomaConocidoes.Remove(idiomaConocido);
            db.SaveChanges();

            return Ok(idiomaConocido);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IdiomaConocidoExists(int id)
        {
            return db.IdiomaConocidoes.Count(e => e.Id == id) > 0;
        }
    }
}