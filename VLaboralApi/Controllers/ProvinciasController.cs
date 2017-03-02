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
    public class ProvinciasController : ApiController
    {
        private VLaboral_Context db = new VLaboral_Context();

        // GET: api/Provincias
        public IQueryable<Provincia> GetProvincias()
        {
            return db.Provincias;
        }

        // GET: api/Provincias/5
        [ResponseType(typeof(Provincia))]
        public IHttpActionResult GetProvincia(int id)
        {
            var provincia = db.Provincias.Find(id);
            if (provincia == null)
            {
                return NotFound();
            }

            return Ok(provincia);
        }

        [ResponseType(typeof(IEnumerable<Ciudad>))]
        [Route("api/Provincias/{provinciaId}/Ciudades")]
        public IHttpActionResult GetCiudades(int provinciaId)
        {
            var ciudades = db.Ciudades.Where(c => c.ProvinciaId == provinciaId);
            return Ok(ciudades);
        }

        // PUT: api/Provincias/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProvincia(int id, Provincia provincia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != provincia.Id)
            {
                return BadRequest();
            }

            db.Entry(provincia).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProvinciaExists(id))
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

        // POST: api/Provincias
        [ResponseType(typeof(Provincia))]
        public IHttpActionResult PostProvincia(Provincia provincia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Provincias.Add(provincia);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = provincia.Id }, provincia);
        }

        // DELETE: api/Provincias/5
        [ResponseType(typeof(Provincia))]
        public IHttpActionResult DeleteProvincia(int id)
        {
            Provincia provincia = db.Provincias.Find(id);
            if (provincia == null)
            {
                return NotFound();
            }

            db.Provincias.Remove(provincia);
            db.SaveChanges();

            return Ok(provincia);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProvinciaExists(int id)
        {
            return db.Provincias.Count(e => e.Id == id) > 0;
        }
    }
}