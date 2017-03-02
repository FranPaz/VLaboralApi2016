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
using VLaboralApi.Models.Ubicacion;

namespace VLaboralApi.Controllers
{
    public class PaisController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/Pais
        public IQueryable<Pais> GetPaises()
        {
            return db.Paises;
        }

        // GET: api/Pais/5
        [ResponseType(typeof(Pais))]
        public IHttpActionResult GetPais(int id)
        {
            var pais = db.Paises.Find(id);
            if (pais == null)
            {
                return NotFound();
            }

            return Ok(pais);
        }

        [ResponseType(typeof(IEnumerable<Provincia>))]
        [Route("api/Paises/{paisId}/Provincias")]
        public IHttpActionResult GetProvincias(int paisId)
        {
            var provincias = db.Provincias.Where(p => p.PaisId == paisId);
            return Ok(provincias);
        }
        
        // PUT: api/Pais/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPais(int id, Pais pais)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pais.Id)
            {
                return BadRequest();
            }

            db.Entry(pais).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaisExists(id))
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

        // POST: api/Pais
        [ResponseType(typeof(Pais))]
        public IHttpActionResult PostPais(Pais pais)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Paises.Add(pais);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pais.Id }, pais);
        }

        // DELETE: api/Pais/5
        [ResponseType(typeof(Pais))]
        public IHttpActionResult DeletePais(int id)
        {
            Pais pais = db.Paises.Find(id);
            if (pais == null)
            {
                return NotFound();
            }

            db.Paises.Remove(pais);
            db.SaveChanges();

            return Ok(pais);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaisExists(int id)
        {
            return db.Paises.Count(e => e.Id == id) > 0;
        }
    }
}